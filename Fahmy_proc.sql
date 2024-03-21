create proc Exam_Generation @ins_id varchar(14), @crs_id int,@tf int
as
begin
	create table #t(ques_id int)
	declare @sql NVARCHAR(MAX);
	declare @mcq3 int = floor((10 - @tf) / 2)
	declare @mcq2 int = 10-(@tf+@mcq3)

	SET @sql = '
		INSERT INTO #t
		SELECT TOP ' + CAST(@tf AS NVARCHAR(10)) + ' i.ques_id
		FROM Instructor_Course_Question i,Question q
		where q.ques_type=''T'' and i.ins_id=@ins_id and i.crs_id=@crs_id and i.ques_id=q.ques_id
		ORDER BY NEWID();
	';
	EXEC sp_executesql @sql, N'@ins_id varchar(14), @crs_id INT', @ins_id, @crs_id;

	SET @sql = '
		INSERT INTO #t
		SELECT TOP ' + CAST(@mcq2 AS NVARCHAR(10)) + ' i.ques_id
		FROM Instructor_Course_Question i,Question q
		where q.ques_type=''M'' and q.ques_weight=2 and i.ins_id=@ins_id and i.crs_id=@crs_id and i.ques_id=q.ques_id
		ORDER BY NEWID();
	';
	EXEC sp_executesql @sql, N'@ins_id varchar(14), @crs_id INT', @ins_id, @crs_id;

	SET @sql = '
		INSERT INTO #t
		SELECT TOP ' + CAST(@mcq3 AS NVARCHAR(10)) + ' i.ques_id
		FROM Instructor_Course_Question i,Question q
		where q.ques_type=''M'' and q.ques_weight=3 and i.ins_id=@ins_id and i.crs_id=@crs_id and i.ques_id=q.ques_id
		ORDER BY NEWID();
	';
	EXEC sp_executesql @sql, N'@ins_id varchar(14), @crs_id INT', @ins_id, @crs_id;

	begin try
	select * from #t
		insert into Exam_Question
		select ques_id
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

exec Exam_Generation 29040512000017,2,2

go

---------------------------------------------------------------------------------------
create proc Exam_Answers @Ex_id int
as
begin
	select q.ques_answer
	from Exam_Question eq,Question q
	where eq.Exam_id=@Ex_id and q.ques_id=eq.Question_id
end

exec Exam_Answers 1

go

----------------------------------------------------------------------------------------
create proc Exam_Correction @Ex_id int,@std_id varchar(14)
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
	set @grade = @Student_Grade_Sum/@Grade_Sum
	exec Add_Grade_To_Student @std_id,@Ex_id,@grade
	select @grade
end

go

-----------------------------------------------------------------------------------------
--branch SP
create proc Read_All_Branches
as
begin
	select * from Branch
end
--
go

create proc Add_Branch @Branch_Name varchar(25), @MgrId varchar(14)
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

create proc Update_Branch @Branch_Id int,@Branch_Name varchar(25), @MgrId varchar(14)
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

create proc Delete_Branch @Branch_Id int
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

create proc Read_All_Students
as
begin
	select * from Student
end
--
go

create proc Add_Student @std_name varchar(25), @std_password varchar(15),@std_mobile varchar(11),@std_birthDate date,@track_id int,@branch_id int
as
begin
	begin try
		insert into Student(std_name,std_password,std_mobile,std_birthDate,track_id,branch_id) values(@std_name,@std_password,@std_mobile,@std_birthDate,@track_id,@branch_id)
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

create proc Update_Student @std_id varchar(14),@std_name varchar(25), @std_password varchar(15),@std_mobile varchar(11),@std_birthDate date,@track_id int,@branch_id int
as
begin
	begin try
		update Student
		set std_name=@std_name, std_password=@std_password,std_mobile=@std_mobile,std_birthDate=@std_birthDate,track_id=@track_id,branch_id=@branch_id
		where std_id=@std_id
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

create proc Delete_Student @std_id varchar(14)
as
begin
	begin try
		delete from Student where std_id=@std_id
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
--------------------------------------------------------------------------------------
go

create proc Add_Grade_To_Student @std_id varchar(14), @Exam_id int,@Grade float
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
