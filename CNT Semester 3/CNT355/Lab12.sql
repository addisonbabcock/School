CREATE PROCEDURE spCalcSMA

--Example INCOMPLETE solution for L12

/*
Simple Moving Average; this is a basic routine that only handles DAILY periods
AND RETURNS MAX. OF 1 YR. DATA */

	@sSymbol varchar(10),
	@sExch varchar(10),
	@PeriodType char(1) = 'D',	--D for daily (default); W for weekly
	@EndDateForCalc smalldatetime,  --SMA is based on x no. of days prior to & incl. this date
	@NumberOfPeriods tinyint, --typical values are 14, 50, 200 days
	@NumberOfMonths tinyint	--specifies APPROXIMATE amount of data to return (optimizes performance)
AS

SET NOCOUNT ON	--kill rowcount msgs

--make sure data exists
IF EXISTS
	(
	SELECT *
	FROM  PricesAndVolume
	WHERE @sSymbol = Symbol AND @sExch = Exchange --+2
	)
BEGIN
	--print 'data exists for equity'
	DECLARE @n tinyint
	DECLARE @SMA smallmoney	--for holding a calc.

	--create table variable to hold close prices in memory
	DECLARE @ClosingPrices Table (
		Date smalldatetime not null,
		Price smallmoney null)

	--copy ALL available equity data from database table to memory
	INSERT @ClosingPrices
			SELECT Date,Cl
			FROM  PricesAndVolume
			WHERE Symbol=@sSymbol AND Exchange=@sExch AND Date<=@EndDateForCalc
			ORDER BY DATE DESC

	--is there enough data to perform first calculation?
	IF (SELECT count (*) FROM  @ClosingPrices)	>= @NumberOfPeriods --+1
	BEGIN
		--init variable used to get right number of values
		SET @n = @NumberOfPeriods

		--create a table variable to hold window of data for calculations
		DECLARE @WindowOfRecords Table (
			Date smalldatetime not null,
			Price smallmoney null)

		--also create a table variable to hold calcualted values
		DECLARE @MovingAverages Table (
			Date smalldatetime not null,
			MovingAverage smallmoney null)

		DECLARE @CurrentDate smalldatetime
		DECLARE @WindowUpperLimitDate smalldatetime
		DECLARE @Flag bit	--use for looping as window of data moves

		WHILE @n>0
		BEGIN
			--get initial window of data into working table
			--working from most recent date to oldest
			INSERT @WindowOfRecords (Date,Price)
				SELECT TOP 1 * 
				FROM @ClosingPrices ORDER BY DATE DESC	--copy a record over

			--delete the row from source table as it is no longer needed
			SET @CurrentDate=(SELECT TOP 1 Date 
					FROM @ClosingPrices ORDER BY DATE DESC)
			DELETE @ClosingPrices WHERE Date = @CurrentDate

			SET @n=@n-1
		END

		--code below will be moving the "window" of data to use; it will hold "n" records at a time
		SET @Flag=1	--init flag; this will be reset whenever there is no longer enough data
		--to fill the window

		WHILE @Flag=1
		BEGIN
			--store average before moving start of window lower limit to next earlier date
			SET @WindowUpperLimitDate=
				(SELECT TOP 1 Date FROM @WindowOfRecords ORDER BY Date DESC)
			SET @SMA=(SELECT AVG(Price) From @WindowOfRecords)

			INSERT @MovingAverages
			VALUES(@WindowUpperLimitDate,@SMA)

			--delete record for latest date in @WindowOfRecords
			DELETE @WindowOfRecords WHERE Date = @WindowUpperLimitDate

			--get next record from @ClosingPrices to bring window back up to size
			INSERT @WindowOfRecords (Date,Price)
				SELECT TOP 1 * FROM @ClosingPrices ORDER BY DATE DESC	--window back up to n records

			--delete the row from source table (@ClosingPrices) as it is no longer needed
			SET @CurrentDate=(SELECT TOP 1 Date FROM @ClosingPrices ORDER BY DATE DESC)
			DELETE @ClosingPrices WHERE Date = @CurrentDate

			--is there enough data to perform another calculation?
			IF (SELECT COUNT(*) FROM  @WindowOfRecords)	< @NumberOfPeriods
				SET @Flag=0
		END
		
		--return data to caller; limit to @NumberOfMonths max of data
		SELECT * FROM @MovingAverages 
		WHERE Date BETWEEN DATEADD(mm,-@NumberOfMonths,@EndDateForCalc) and @EndDateForCalc
		
		RETURN @@ERROR	
	END
	ELSE
	BEGIN
		--Not enough data for equity to perform any calculations'
		RETURN 2
	END
END
ELSE
BEGIN
	--NO DATA in database for specified equity'
	RETURN 1
END
