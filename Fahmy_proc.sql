alter proc Exam_Generation @ins_id varchar(14), @crs_id int, @tf int, @duration int
as
begin
	create table #t(ques_id int)
	declare @sql NVARCHAR(MAX);
	declare @mcq3 int = floor((10 - @tf) / 2)
	declare @mcq2 int = 10-(@tf+@mcq3)

	SET @sql = '
		INSERT INTO #t
		SELECT TOP ' + CAST(@tf AS NVARCHAR(10)) + ' q.ques_id
		FROM Question q
		where q.ques_type=''T'' and q.ins_id=@ins_id and q.crs_id=@crs_id
		ORDER BY NEWID();
	';
	EXEC sp_executesql @sql, N'@ins_id varchar(14), @crs_id INT', @ins_id, @crs_id;

	SET @sql = '
		INSERT INTO #t
		SELECT TOP ' + CAST(@mcq2 AS NVARCHAR(10)) + ' q.ques_id
		FROM Question q
		where q.ques_type=''M'' and q.ques_weight=2 and q.ins_id=@ins_id and q.crs_id=@crs_id 
		ORDER BY NEWID();
	';
	EXEC sp_executesql @sql, N'@ins_id varchar(14), @crs_id INT', @ins_id, @crs_id;

	SET @sql = '
		INSERT INTO #t
		SELECT TOP ' + CAST(@mcq3 AS NVARCHAR(10)) + ' q.ques_id
		FROM Question q
		where q.ques_type=''M'' and q.ques_weight=3 and q.ins_id=@ins_id and q.crs_id=@crs_id 
		ORDER BY NEWID();
	';
	EXEC sp_executesql @sql, N'@ins_id varchar(14), @crs_id INT', @ins_id, @crs_id;

	begin try
		declare @Ex_Id int, @Exam_Grade int = @tf+(@mcq2*2)+(@mcq3*3)
		create table #last_Ex_Id(Ex_id int)
		insert into Exam(Ex_duration,Ex_grade,Ex_passGrade,crs_id) 
		OUTPUT INSERTED.Ex_id INTO #last_Ex_Id
		values(@duration,@Exam_Grade,@Exam_Grade/2,@crs_id)
		select @Ex_Id = Ex_id from #last_Ex_Id
		drop table #last_Ex_Id
		select @Ex_Id

		insert into Exam_Question
		select @Ex_Id, ques_id
		from #t
		if(@@ROWCOUNT = 0)
			throw 50000, 'Exam already exist', 2;
	end try
	begin catch
		select ERROR_MESSAGE() as ErrorNumber;
		insert into 
			error_log(errorNumber, errorMessage, errorProcedure, errorTime)
		values 
			(ERROR_NUMBER(), ERROR_MESSAGE(), OBJECT_NAME(@@PROCID), GETDATE())
	end catch
	
end

exec Exam_Generation '29040512000017',2,4,10;

go

---------------------------------------------------------------------------------------
alter proc Exam_Answers @Ex_id int
as
begin
	select q.ques_answer
	from Exam_Question eq,Question q
	where eq.Exam_id=@Ex_id and q.ques_id=eq.Question_id
end

exec Exam_Answers 1

go

----------------------------------------------------------------------------------------
alter proc Exam_Correction @Ex_id int,@std_id varchar(14)
as
begin
	declare c1 cursor
	for select ste.answer,q.ques_answer,q.ques_weight
		from Student_Take_Exam ste, Question q
		where ste.Question_id=q.ques_id
	for read only

	declare @Student_Answer varchar(1), @Answer varchar(1),@Question_Weight int, @Student_Grade_Sum int = 0,@Grade_Sum int = 0
	open c1
	fetch c1 into @Student_Answer,@Answer,@Question_Weight
	while @@FETCH_STATUS=0
		begin
			if @Student_Answer=@Answer
				set @Student_Grade_Sum+=@Question_Weight
			set @Grade_Sum+=@Question_Weight

			select @Student_Answer,@Answer,@Question_Weight
			fetch c1 into @Student_Answer,@Answer,@Question_Weight
		end
	close c1
	deallocate c1
	
	declare @grade float
	set @grade = @Student_Grade_Sum
	exec Add_Grade_To_Student @std_id,@Ex_id,@grade
	select @grade
end

go

-----------------------------------------------------------------------------------------
--branch SP
alter proc Read_All_Branches
as
begin
	select * from Branch
end
--
go

alter proc Add_Branch @Branch_Name varchar(25), @MgrId varchar(14)
as
begin
	begin try
		insert into Branch(BranchName,MgrId) values(@Branch_Name,@MgrId)
		if(@@ROWCOUNT = 0)
			throw 40000, 'Invalid Data', 2;
	end try
	begin catch
		select error_number() as ErrorNumber;
		insert into 
			error_log(errorNumber, errorMessage, errorProcedure, errorTime)
		values 
			(ERROR_NUMBER(), ERROR_MESSAGE(), OBJECT_NAME(@@PROCID), GETDATE())
	end catch
end
--
go

alter proc Update_Branch @Branch_Id int,@Branch_Name varchar(25), @MgrId varchar(14)
as
begin
	begin try
		update Branch
		set BranchName = @Branch_Name,MgrId = @MgrId
		where BranchId=@Branch_Id
		if(@@ROWCOUNT = 0)
			throw 50000, 'Invalid Data', 1;
	end try
	begin catch
		select error_number() as ErrorNumber;
		insert into 
			error_log(errorNumber, errorMessage, errorProcedure, errorTime)
		values 
			(ERROR_NUMBER(), ERROR_MESSAGE(), OBJECT_NAME(@@PROCID), GETDATE())
	end catch
end
--
go

alter proc Delete_Branch @Branch_Id int
as
begin
	begin try
		delete from Branch where BranchId=@Branch_Id
		if @@ROWCOUNT = 0
			throw 50000, 'Course not found', 1
	end try
	begin catch
		select error_number() as ErrorNumber;
		insert into 
			error_log(errorNumber, errorMessage, errorProcedure, errorTime)
		values 
			(ERROR_NUMBER(), ERROR_MESSAGE(), OBJECT_NAME(@@PROCID), GETDATE())
	end catch
end
-------------------------------------------------------------------------------------------
----student SP
go

alter proc Read_All_Students
as
begin
	select * from Student
end
--
go

alter proc Add_Student @std_Id varchar(14), @std_name varchar(25), @std_password varchar(15),@std_mobile varchar(11),@std_birthDate date,@track_id int,@branch_id int
as
begin
	begin try
		begin transaction
		insert into Users(ID, Name, Password, Mobile, RoleId)
		values(@std_Id, @std_name, @std_password, @std_mobile, 1);
		insert into Student(std_id, std_birthDate, branch_id, track_id)
		values(@std_Id, @std_birthDate, @branch_id, @track_id);
		if(@@ROWCOUNT = 0)
			throw 40000, 'Invalid Data', 2;
		commit;
	end try
	begin catch
		rollback;
		select error_number() as ErrorNumber;
		insert into 
			error_log(errorNumber, errorMessage, errorProcedure, errorTime)
		values 
			(ERROR_NUMBER(), ERROR_MESSAGE(), OBJECT_NAME(@@PROCID), GETDATE())
	end catch
end
--
go

alter proc Update_Student @std_id varchar(14),@std_name varchar(25), @std_password varchar(15),@std_mobile varchar(11),@std_birthDate date,@track_id int,@branch_id int
as
begin
	begin try
		begin transaction
		--update Student
		--set std_name=@std_name, std_password=@std_password,std_mobile=@std_mobile,std_birthDate=@std_birthDate,track_id=@track_id,branch_id=@branch_id
		--where std_id=@std_id;
		update Student
		set branch_id = @branch_id, track_id = @track_id, std_birthDate = @std_birthDate
		where std_id = @std_id;
		update Users
		set Name = @std_name, Password = @std_password, Mobile = @std_mobile
		where Users.ID = @std_id;
		if(@@ROWCOUNT = 0)
			throw 50000, 'Invalid Data', 1;
		commit;
	end try
	begin catch
		rollback;
		select error_number() as ErrorNumber;
		insert into 
			error_log(errorNumber, errorMessage, errorProcedure, errorTime)
		values 
			(ERROR_NUMBER(), ERROR_MESSAGE(), OBJECT_NAME(@@PROCID), GETDATE())
	end catch
end
--
go

alter proc Delete_Student @std_id varchar(14)
as
begin
	begin try
		begin transaction
		delete from Student where std_id=@std_id;
		delete from Users where Users.ID = @std_id;
		if @@ROWCOUNT = 0
			throw 50000, 'Course not found', 1;
		commit;
	end try
	begin catch
	rollback;
		select error_number() as ErrorNumber;
		insert into 
			error_log(errorNumber, errorMessage, errorProcedure, errorTime)
		values 
			(ERROR_NUMBER(), ERROR_MESSAGE(), OBJECT_NAME(@@PROCID), GETDATE())
	end catch
end
--------------------------------------------------------------------------------------
go

alter proc Add_Grade_To_Student @std_id varchar(14), @Exam_id int,@Grade float
as
begin
	declare @exist int

	select @exist = count(std_id)
	from Student_Exam_Grade
	where std_id=@std_id and Exam_id=@Exam_id

	if @exist=0
		begin
			insert into Student_Exam_Grade values(@std_id,@Exam_id,@Grade)
		end
	else
		begin
			update Student_Exam_Grade
			set grade=@Grade
			where std_id=@std_id and Exam_id=@Exam_id
		end
end

go 


alter proc Read_Questions_With_Students_Answers @examId int, @studentId varchar(14)
as
begin
	create table #t(ques_tittle varchar(max),Choices varchar(max),student_answer varchar(1),model_answer varchar(1))

	declare c1 cursor
	for select ste.Question_id,q.ques_tittle,ste.answer,q.ques_answer,q.ques_type
		from Student_Take_Exam ste, Question q
		where ste.Exam_id=@examId and ste.std_id=@studentId and ste.Question_id=q.ques_id
	for read only

	declare @ques_id int,@ques_tittle varchar(max), @student_answer varchar(1), @ques_answer varchar(1),@ques_type varchar(1)
	open c1
	fetch c1 into @ques_id,@ques_tittle,@student_answer,@ques_answer,@ques_type
	while @@FETCH_STATUS=0
		begin
			if @ques_type='M'
				begin
					insert into #t
					select @ques_tittle,CONCAT_WS(', ', c.A, c.B, c.C, c.D) as 'Choices',@student_answer as 'Student Answer', @ques_answer as 'Model Answer'
					from Choice c
					where c.ques_id=@ques_id
				end
			else if @ques_type='T'
				begin
					insert into #t
					select @ques_tittle,CONCAT_WS(', ', 'True', 'False') as 'Choices',@student_answer as 'Student Answer', @ques_answer as 'Model Answer'
				end

			fetch c1 into @ques_id,@ques_tittle,@student_answer,@ques_answer,@ques_type
		end
	close c1
	deallocate c1
	select * from #t
end

go 


alter proc Read_Questions_With_Students_Answers @examId int, @studentId varchar(14)
as
begin
	create table #t(ques_tittle varchar(max),Choices varchar(max),student_answer varchar(1),model_answer varchar(1))

	declare c1 cursor
	for select ste.Question_id,q.ques_tittle,ste.answer,q.ques_answer,q.ques_type
		from Student_Take_Exam ste, Question q
		where ste.Exam_id=@examId and ste.std_id=@studentId and ste.Question_id=q.ques_id
	for read only

	declare @ques_id int,@ques_tittle varchar(max), @student_answer varchar(1), @ques_answer varchar(1),@ques_type varchar(1),@num int = 1
	open c1
	fetch c1 into @ques_id,@ques_tittle,@student_answer,@ques_answer,@ques_type
	while @@FETCH_STATUS=0
		begin
			if @ques_type='M'
				begin
					insert into #t
					select CONCAT_WS('- ',@num,@ques_tittle),CONCAT_WS(', ', c.A, c.B, c.C, c.D) as 'Choices',@student_answer as 'Student Answer', @ques_answer as 'Model Answer'
					from Choice c
					where c.ques_id=@ques_id
				end
			else if @ques_type='T'
				begin
					insert into #t
					select CONCAT_WS('- ',@num,@ques_tittle),CONCAT_WS(', ', 'True', 'False') as 'Choices',@student_answer as 'Student Answer', @ques_answer as 'Model Answer'
				end
			set @num = @num+1
			fetch c1 into @ques_id,@ques_tittle,@student_answer,@ques_answer,@ques_type
		end
	close c1
	deallocate c1
	select * from #t
end

exec Read_Questions_With_Students_Answers 1,12345678901234

alter proc Read_Student_Grades_By_Student_Id @studentId varchar(14)
as
begin
	begin try
		select c.crs_name,seg.grade
		from Student_Exam_Grade seg,Exam e,Course c
		where seg.Exam_id=e.Ex_id and e.crs_id=c.crs_id
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end

exec Read_Student_Grades_By_Student_Id 13579246801357

exec Read_Instructor_Courses_By_Instructor_Id 29040512000017

exec Read_Exam_Questions 11
