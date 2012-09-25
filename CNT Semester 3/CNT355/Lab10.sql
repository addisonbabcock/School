use lab10

create view vwQ1
as
	select	class_trak..classes.class_id 'Class ID',
			class_trak..classes.class_desc 'Class Description',
			count (*) 'Students'
	from	class_trak..classes, class_trak..class_to_student
	where	class_trak..classes.class_id = class_trak..class_to_student.class_id
	group by class_trak..classes.class_id, class_trak..classes.class_desc

create view vwQ2
as
	select	class_trak..courses.course_id 'Course ID',
			class_trak..courses.course_abbrev 'Abbreviation',
			class_trak..courses.course_desc 'Description',
			count (*) 'Classes'
	from	class_trak..courses, class_trak..classes
	where	class_trak..courses.course_id = class_trak..classes.course_id
	group by class_trak..courses.course_id, 
			class_trak..courses.course_abbrev, 
			class_trak..courses.course_desc

create view vwQ3
as
	select	class_trak..classes.class_id 'Class ID',
			class_trak..classes.class_desc 'Description',
			count (*) 'Assigments'
	from	class_trak..classes, class_trak..requirements
	where	class_trak..classes.class_id = class_trak..requirements.class_id and
			class_trak..requirements.ass_type_id = 1
	group by class_trak..classes.class_id,
			class_trak..classes.class_desc

create proc spQ4
	@ClassID	int
as
	select	*
	from	vwQ3
	where	[Class ID] = @ClassID
return @@ERROR

exec spQ4 36

create view vwQ5
as
	select	class_trak..classes.class_id 'Class ID',
			class_trak..classes.course_id 'Course ID',
			avg (class_trak..results.score / class_trak..requirements.max_score) * 100
				'Average exam score'
	from	class_trak..classes, 
			class_trak..results,
			class_trak..requirements
	where	class_trak..classes.class_id = class_trak..results.class_id and
			class_trak..classes.class_id = class_trak..requirements.class_id and
			class_trak..requirements.ass_type_id in (3, 4, 5)
	group by class_trak..classes.class_id,
			class_trak..classes.course_id

create proc spQ5
	@ClassID	int
as
	select	*
	from	vwQ5
	where	[Class ID] = @classid

	if @@ERROR <> 0
	begin
		return 1
	end
	else
	begin
		return 0
	end

exec spQ5 36

--Ass
--20	36	C-3		50	0	71
--Labs
--20	36	DOS-2	13	0	14

create view GetAssignments
as
	select	class_trak..requirements