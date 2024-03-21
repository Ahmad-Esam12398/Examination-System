create table error_log
(
	id int primary key identity,
	errorNumber int,
	errorMessage nvarchar(4000),
	errorProcedure nvarchar(200),
	errorTime datetime
)

go

create proc Log_Error
as
begin
	insert into 
		error_log(errorNumber, errorMessage, errorProcedure, errorTime)
	values 
		(ERROR_NUMBER(), ERROR_MESSAGE(), OBJECT_NAME(@@PROCID), GETDATE());
end

go

create proc Show_Error
as
begin
	select ERROR_NUMBER();
end

go

create proc Throw_Error_No_Rows_Affected
as
begin
	if @@ROWCOUNT = 0
		throw 50000, 'Course not found', 1
end

go

create proc Add_Course @courseName varchar(20), @courseDuration int
as
begin
	begin try
		insert into Course(crs_name, crs_duration) values (@courseName, @courseDuration)
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Delete_Course_By_Id @id int
as
begin
	begin try
		delete from Course where crs_id = @id;
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Update_Course_By_Id @id int, @courseName varchar(20), @courseDuration int
as
begin
	begin try
		update Course set crs_name = @courseName, crs_duration = @courseDuration
		where crs_id = @id
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Read_All_Courses 
as
begin
select * from Course
end

go

create proc Assign_Course_To_Instructor_Track_Branch @instructorId nvarchar(14), @course_Id int, @trackId int, @branchId int
as
begin
	begin try
		insert into 
			Instructor_Teach_Course_For_Track_In_Branch(ins_id, crs_id, track_id, branch_id)
		values (@instructorId, @course_Id, @trackId, @branchId);
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Delete_Course_From_Instructor @instructorId nvarchar(14), @course_id int
as
begin
	begin try
		delete from 
			Instructor_Teach_Course_For_Track_In_Branch
		where 
			ins_id = @instructorId and crs_id = @course_id
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Read_All_Instructor_Courses @instructorId nvarchar(14)
as
begin
	begin try
		select ic.crs_id, c.crs_name 
		from Instructor_Teach_Course_For_Track_In_Branch ic
		join Course c
		on ic.crs_id = c.crs_id
		where ic.ins_id = @instructorId
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch

end

go

create proc Read_All_Topics
as
begin
	select t.top_id, t.top_name
	from Topic t
end

go

create proc Add_Topic @topicName varchar(20), @courseId int
as
begin
	begin try
		insert into 
		Topic(top_name)
		values
		(@topicName, @courseId)
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Update_Topic @topicId int, @topicName varchar(20), @courseId int
as
begin
	begin try
		update Topic set top_name = @topicName
		where top_id = @topicId;
		update Course_Topic set courseId = @courseId
		where topicId = @topicId;
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Delete_Topic @topicId int
as
begin
	begin try
		delete from Topic where top_id = @topicId
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Assign_Course_To_Track @courseId int, @trackId int
as
begin
	begin try
		insert into 
			Track_Course(track_id, crs_id)
		values
			(@trackId, @courseId);
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Delete_Course_From_Track @courseId int, @trackId int
as
begin
	begin try
		delete from 
			Track_Course 
		where 
			crs_id = @courseId and track_id = @trackId
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Read_All_Track_Courses @trackId int
as
begin
	begin try
		select tc.crs_id, c.crs_name 
		from Track_Course tc
		join Course c
		on tc.crs_id = c.crs_id
		where tc.track_id = @trackId;
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch

end

go

create proc Add_Question @Qtitle varchar(200), @QAnswer char, @Qtype char, @Qweight int, @Qchoice1 varchar(100), @Qchoice2 varchar(100), @Qchoice3 varchar(100), @Qchoice4 varchar(100), @instructorId int, @crs_id int
as
begin
	begin try
		if(@QAnswer not in ('A', 'B', 'C', 'D'))
			throw 5000, 'Invalid Answer', 1;
		else if(@Qtype not in ('M', 'T'))
			throw 5000, 'Invalid Type', 1;
		else if(@Qweight not in(1, 2, 3))
			throw 5000, 'Invalid Weight', 1;
		else
		begin
			begin tran
			insert into 
			Question(ques_tittle, ques_type, ques_weight, ques_answer, ins_id, crs_id)
			values
			(@Qtitle, @Qtype, @Qweight, @QAnswer, @instructorId, @crs_id);

			insert into
			Choice(ques_id, A, B, C, D)
			values
			(@@IDENTITY, @Qchoice1, @Qchoice2, @Qchoice3, @Qchoice4);
			exec Throw_Error_No_Rows_Affected;
			commit tran
		end
	end try
	begin catch
		rollback tran
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Delete_Question @QId int
as
begin
	begin try
		begin tran
		delete from Question where ques_id = @QId;
		delete from Choice where ques_id = @QId;
		exec Throw_Error_No_Rows_Affected;
		commit tran
	end try
	begin catch
		rollback tran
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Update_Question @QId int, @Qtitle varchar(200), @QAnswer char, @Qtype char, @Qweight int, @Qchoice1 varchar(100), @Qchoice2 varchar(100), @Qchoice3 varchar(100), @Qchoice4 varchar(100), @instructorId int, @crs_id int
as
begin
	begin try
		if(@QAnswer not in ('A', 'B', 'C', 'D'))
			throw 5000, 'Invalid Answer', 1;
		else if(@Qtype not in ('M', 'T'))
			throw 5000, 'Invalid Type', 1;
		else if(@Qweight not in(1, 2, 3))
			throw 5000, 'Invalid Weight', 1;
		else
		begin
			begin tran
			update  
			Question
			set
			ques_tittle = @Qtitle, ques_type = @Qtype, ques_weight = @Qweight, ques_answer = @QAnswer,
			ins_id = @instructorId, crs_id = @crs_id
			where ques_id = @QId;
			update
			Choice(ques_id, A, B, C, D)
			set
			ques_id = @@IDENTITY, A = @Qchoice1, B = @Qchoice2, C = @Qchoice3, D = @Qchoice4
			where ques_id = @QId;
			exec Throw_Error_No_Rows_Affected;
			commit tran;
		end
	end try
	begin catch
		rollback tran;
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Read_All_Questions
as
begin
	select *
	from Question q
	join Choice c
	on c.ques_id = q.ques_id
end

go

--create proc Assgin_Question_For_Course_By_Instructor @QId int, @CourseId int, @InstructorId varchar(14)
--as
--begin
--	begin try
--	insert into 
--		Instructor_Course_Question(ques_id, crs_id, ins_id)
--	values
--		(@QId, @CourseId, @InstructorId);
--	exec Throw_Error_No_Rows_Affected;
--	end try
--	begin catch
--		exec Show_Error;
--		exec Log_Error;
--	end catch
--end

go

--create proc Delete_Question_From_Course_By_Instructor @QId int
--as
--begin
--begin try
--	delete from
--		Instructor_Course_Question
--	where 
--		ques_id = @QId and crs_id = @CourseId and ins_id = @InstructorId;
--	exec Throw_Error_No_Rows_Affected;
--end try
--begin catch
--	exec Show_Error;
--	exec Log_Error;
--end catch
--end

--go

create proc Read_All_Questions_For_Course_By_Instructor @InstructorId varchar(14), @courseId int
as
begin
	select i.Ins_id, c.crs_id, c.crs_name, q.ques_tittle, q.ques_answer,
	q.ques_tittle, ch.A, ch.B, ch.C, ch.D

	from Question q
	join Course c
	on c.crs_id = q.crs_id
	join Instructor i
	on i.Ins_id = q.ins_id
	join Choice ch
	on ch.ques_id = q.ques_id
	where
	q.ins_id = @InstructorId and q.crs_id = @courseId;
end

go

create proc Read_All_Questions_For_Course @courseId int
as
begin
	select c.crs_name, q.ques_tittle, ch.A, ch.B, ch.C, ch.D, q.ques_answer, q.ques_weight 
	from Question q
	join Course c
	on c.crs_id = q.crs_id
	join Choice ch
	on ch.ques_id = q.ques_id
	where q.crs_id = @courseId
end

go

-- Reports stored procedures

create proc Read_Students_Data_By_Track_Id @track_id int
as
begin
begin try
	select s.std_id, s.std_name, s.std_mobile, s.std_birthDate, t.track_name
	from Student s
	join Track t
	on t.track_id = s.track_id
	where t.track_id = @track_id
end try
begin catch
	exec Show_Error;
	exec Log_Error;
end catch
end

go

create proc Read_Student_Grades_By_Student_Id @studentId int
as
begin
	begin try
		select s.std_name, c.crs_name, se.Exam_id
		from Student_Exam_Grade se
		join Student s
		on s.std_id = se.std_id
		join Track_Course_Exam tce
		on tce.Exam_id = se.Exam_id
		join Course c
		on c.crs_id = tce.crs_id
		where se.std_id = @studentId
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Read_Instructor_Courses_By_Instructor_Id @instructorId int
as
begin
	begin try
		select 
		c.crs_name, t.track_name, b.BranchName, count(s.std_id) as 'Number Of Students'
		from Instructor i
		join Instructor_Teach_Course_For_Track_In_Branch itc
		on itc.ins_id = i.Ins_id
		join Course c
		on c.crs_id = itc.crs_id
		join Student s
		on s.track_id = itc.track_id and s.branch_id = itc.branch_id
		join Track t
		on t.track_id = itc.track_id
		join Branch b
		on itc.branch_id = b.BranchId
		where itc.ins_id = @instructorId
		group by c.crs_name, t.track_name, b.BranchName
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Read_Topics_Of_Course @courseId int
as
begin
	begin try
		select c.crs_name, t.top_name
		from Course c
		join Course_Topic ct
		on ct.courseId = c.crs_id
		join Topic t
		on t.top_id = ct.topicId
		where c.crs_id = @courseId
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

go

create proc Read_Exam_Questions @ExamId int
as
begin
	select q.ques_id, q.ques_tittle, 
	case
		when q.ques_type = 'M' then CONCAT_WS(', ', c.A, c.B, c.C, c.D)
		when q.ques_type = 'T' then CONCAT_WS(', ', 'True', 'False')
	end as 'Choices'
	from Exam_Question eq
	join Question q
	on q.ques_id = eq.Question_id
	left join Choice c
	on c.ques_id = q.ques_id
	where eq.Exam_id = @ExamId
end

go

create proc Read_Questions_With_Students_Answers @examId int, @studentId int
as
begin
	select ste.Question_id, q.ques_tittle, 
	case
		when q.ques_type = 'M' then CONCAT_WS(', ', c.A, c.B, c.C, c.D)
		when q.ques_type = 'T' then CONCAT_WS(', ', 'True', 'False')
	end as 'Choices',
	ste.answer as 'Student Answer', q.ques_answer as 'Model Answer'
	from Student_Take_Exam ste
	join Question q
	on q.ques_id = ste.Question_id
	join Choice c
	on c.ques_id = ste.Question_id
end










