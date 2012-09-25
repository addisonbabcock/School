use class_trak

--Q1
create proc Question1 as
	select	last_name + ', ' + first_name 'Student Name'
	from	students
	order by	last_name asc
	return	@@ERROR

--Q2
create proc Question2 as
	select	*
	from	classes
	order by class_id, course_id
	return @@ERROR

--Q3
create proc Question3
	@StudID	int
as
	select	students.first_name + ' ' + students.last_name 'Name',
			students.student_id 'Student ID',
			classes.class_desc 'Class Description',
			classes.class_id 'Class ID',
			courses.course_abbrev 'Course Abbreviation',
			courses.course_desc 'Course Description'
	from students
		join class_to_student
		on students.student_id = class_to_student.student_id
			join classes
			on classes.class_id = class_to_student.class_id
				join courses
				on courses.course_id = classes.course_id
	where students.student_id = @StudID
	order by students.last_name, students.first_name
	return @@ERROR

--Q4
create proc Question4
	@StudID int,
	@ClassID int
as
	select	students.last_name + ', ' + students.first_name 'Name',
			classes.class_id 'Class ID',
			classes.class_desc 'Class Description',
			requirements.ass_number 'Assignment Number',
			requirements.ass_desc 'Assigment Description',
			requirements.total_weight 'Weighting'
	from students
		join class_to_student
		on students.student_id = class_to_student.student_id
			join classes
			on classes.class_id = class_to_student.class_id
				join requirements
				on requirements.class_id = classes.class_id
	where	students.student_id = @StudID and
			classes.class_id = @ClassID
	order by requirements.ass_number desc
return @@ERROR

--Q5
create proc Question5
	@StudID int,
	@ClassID int
as
	select	students.last_name + ', ' + students.first_name 'Name',
			courses.course_id 'Course ID',
			courses.course_desc 'Course Description',
			requirements.ass_number 'Assignment Number',
			requirements.ass_desc 'Assigment Description',
			requirements.total_weight 'Weighting'
	from students
		join class_to_student
		on students.student_id = class_to_student.student_id
			join classes
			on classes.class_id = class_to_student.class_id
				join requirements
				on requirements.class_id = classes.class_id
					join courses
					on classes.course_id = courses.course_id
	where	students.student_id = @StudID and
			classes.class_id = @ClassID and
			requirements.ass_type_id = 2
	order by requirements.ass_number asc
return @@ERROR

--Q6
create proc Question6
	@FName varchar (50),
	@LName varchar (50)
as
	select	instructors.instructor_id 'Instructor ID',
			instructors.first_name + ' ' + instructors.last_name 'Name',
			classes.class_desc 'Class Description',
			courses.course_desc 'Course Description',
			classes.start_date 'Start date'
	from instructors
		join classes
		on classes.instructor_id = instructors.instructor_id
			join courses
			on courses.course_id = classes.course_id
	where	instructors.first_name = @FName and
			instructors.last_name = @LName
	order by classes.start_date desc
return @@ERROR

--Q7
create proc Question7
	@StudID int,
	@ClassId int
as
	select	classes.class_id 'Class ID',
			classes.class_desc 'Class Description',
			requirements.ass_desc 'Description of Work',
			results.score 'Score',
			results.penalty 'Penalty',
			requirements.max_score 'Max Score'
	from students
		join results
		on students.student_id = results.student_id
			join requirements
			on requirements.req_id = results.req_id
				join classes
				on requirements.class_id = classes.class_id
	where	students.student_id = @StudID and
			classes.class_id = @ClassID
	order by requirements.ass_type_id asc
return @@ERROR

--Q8
create proc Question8
	@StudID int,
	@ClassId int
as
	select	students.student_id 'Student ID',
			students.first_name + ' ' + students.last_name 'Name',
			cast (avg (results.score) / avg (requirements.max_score) * 100 as decimal (3, 1)) 'Exam AVG'
	from students
		join results
		on students.student_id = results.student_id
			join requirements
			on requirements.req_id = results.req_id
				join classes
				on requirements.class_id = classes.class_id
	where	students.student_id = @StudID and
			classes.class_id = @ClassID and
			requirements.ass_type_id in (3, 4)
	group by students.student_id, students.first_name + ' ' + students.last_name
return @@ERROR

--Q9
create proc Question9
	@StudID int,
	@ClassId int
as
	select	students.student_id 'Student ID',
			students.last_name + ', ' + students.first_name 'Name',
			cast (avg (results.score) / avg (requirements.max_score) * 100 as decimal (3, 1)) 'Assignment AVG',
			cast (avg (results.score) / avg (requirements.max_score) * 10 as decimal (3, 1)) 'Accrued marks'
	from students
		join results
		on students.student_id = results.student_id
			join requirements
			on requirements.req_id = results.req_id
				join classes
				on requirements.class_id = classes.class_id
	where	students.student_id = @StudID and
			classes.class_id = @ClassID and
			requirements.ass_type_id = 1
	group by students.first_name, students.last_name, students.student_id
return @@ERROR