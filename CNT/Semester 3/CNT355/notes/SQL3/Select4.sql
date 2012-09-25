/* Select4.sql

-- more on SELECT statement... */

use Pubs --use Pubs database unless specified otherwise for these examples

/* BETWEEN operator - SEE BKS ONLINE! ...Here is example of using WHERE clause 
with BETWEEN...first let's take a look at a sample table to work with... */

exec sp_help Sales /* to see what the table looks like! */
/* or SELECT * from Sales */
go

/* --------------next batch---------------------  */
PRINT '' /* line break */
PRINT 'Records containing quantities between 40 and 75 INCLUSIVE:'
PRINT ''
SELECT *
FROM Sales
WHERE Qty BETWEEN 40 AND 75
go

/* --------------next batch---------------------  
Books online shows syntax for BETWEEN opertor allows for a negated result, ie if
we slightly modify the above batch... */
PRINT '' /* line break */
PRINT 'Records containing quantities between 40 and 75 INCLUSIVE:'
PRINT ''
SELECT *
FROM Sales
WHERE Qty NOT BETWEEN 40 AND 75
go

/* -----------------IN - see BKS online, IN (T-SQL) info */
PRINT '' /* line break */
PRINT 'Records containing quantities of 40 or 50 ONLY:'
PRINT ''
SELECT *
FROM Sales
WHERE Qty IN (40,50)
go

/* ----------------- more examples...   -------------*/
USE pubs
SET NOCOUNT ON /* we don't need a count of the records! */
SELECT au_lname, state
FROM authors
WHERE state = 'CA' OR state = 'MD'
go

PRINT 'Note the the same results can be obtained using IN:'
PRINT ''
SELECT au_lname, state
FROM authors
WHERE state IN ('CA','MD')
go
/* -----------------------------------------------   
IN can be used with a "subquery" as noted in books online.  The subquery
has a result set of one column, and this defines the set of values used with IN.
For example... */
  
USE pubs
SELECT au_lname, au_fname
FROM authors
WHERE au_id IN
    (SELECT au_id
    FROM titleauthor
    WHERE royaltyper < 50)
go

/* -----------------------------------------------   
IN can be used with the NOT operator, for example... */
USE pubs
SELECT au_lname, au_fname, au_id
FROM authors
WHERE au_lname NOT IN ('Bennet','Carson') /* to exclude these 2 authors */

/* example combining above 2 concepts... */
USE pubs
SELECT au_lname, au_fname, au_id
FROM authors
WHERE au_id NOT IN
    (SELECT au_id
    FROM titleauthor
    WHERE royaltyper < 50)
go

/* ----------------------------------------------------------------------------- 
More on the LIKE clause...  see Books Online, LIKE (T-SQL) where several examples are
shown.  Perhaps most importantly...

i) _ represents a single character
ii) %  represents a string of any length including zero
iii) [] is used to define any SINGLE character in a range of values
iv) [^] is used to define any SINGLE character NOT in the specified range of values

... now for a few examples: -------------------------------------------------------- */

use Pubs
SELECT Title_id,Title
FROM Titles
WHERE Title LIKE '%computers%' /* search for titles containing "computers" */
go

use Pubs
SELECT au_id,au_lname,au_fname
FROM Authors
WHERE au_lname LIKE '[bm]%' /* search for names starting with "b" OR "m"
Note.. the square brackets define a "set" of possible values.  */
go

use Pubs
SELECT au_id,au_lname,au_fname
FROM Authors
WHERE au_lname LIKE '%er'  /* search for names ending in "er"  */
go

use Pubs
SELECT au_id,au_lname,au_fname
FROM Authors
WHERE au_fname LIKE '_ean'  /* pattern match... _ represents any character */
go


/* MISC VARIATIONS...

  WHERE au_fname LIKE '[ck]atherine' would list catherine or katherine values 
  WHERE au_fname LIKE '[m-z]ilson' would return all names with m thru z as the first letter and
  "ilson" after that.

  WHERE au_fname LIKE 'M[^e]%'  would return all names starting with "M" with second letter
  NOT equal to "e".

*/

