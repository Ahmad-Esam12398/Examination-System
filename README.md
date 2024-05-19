# Examination System Data Dictionary

## Stored Procedures

> [!note]
> ## our approach:
> - **We have created a stored procedure for each operation in the system.**
> - **We have created a stored procedure for each report in the system.**
> - **in each stored procedure an Error handling mechanism is implemented to log the error in the error_log table.**
> - **We have created a stored procedure to throw an error if no rows are affected.**
> - **We have created a stored procedure to show the error number to balance between showing the complete error message to user and not showing anything for security reasons.**

### Log_Error 

```sql
create proc Log_Error
as
begin
	insert into 
		error_log(errorNumber, errorMessage, errorProcedure, errorTime)
	values 
		(ERROR_NUMBER(), ERROR_MESSAGE(), OBJECT_NAME(@@PROCID), GETDATE());
end
```

> [!tip] Usage 
It Logs the Error in the Error_Log Table

### Show_Error

```sql
create proc Show_Error
as
begin
	select ERROR_NUMBER();
end
```
### Throw Error If No Rows Affected

```sql
create proc Throw_Error_No_Rows_Affected
as
begin
	if @@ROWCOUNT = 0
		throw 50000, 'Course not found', 1
end
```

> [!tip] Usage
Throws an error if no rows are affected

### AddCourse

```sql
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
```

> [!success] Usage
Adds a course to the Course Table

### DeleteCourse

```sql
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
```

> [!danger] Usage
Deletes a course from the Course Table by Id

### UpdateCourse

```sql
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
```

> [!warning] Usage
Updates a course in the Course Table by Id

### Read All Courses

```sql
create proc Read_All_Courses 
as
begin
select * from Course
end
```

> [!info] Usage
Reads all courses from the Course Table

### Assign Course To Instructor

```sql
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
```

> [!note] Usage
Assigns a course to an instructor for a track in a branch

### Delete Course From Instructor

```sql
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
```

> [!tldr] Usage
Deletes a course from an instructor

### Read All Courses For Instructor

```sql
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
```

> [!tldr] Usage
Reads all courses for an instructor

### Read All Topics

```sql
create proc Read_All_Topics
as
begin
	select t.top_id, t.top_name
	from Topic t
end
```

> [!info] Usage
Reads all topics from the Topic Table

### Add Topic

```sql
create proc Read_All_Topics
as
begin
	select t.top_id, t.top_name
	from Topic t
end
```

> [!success] Usage
Adds a topic to the Topic Table

### Update Topic

```sql
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
```

> [!warning] Usage
Updates a topic in the Topic Table

### Delete Topic

```sql
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
```

> [!danger] Usage
Deletes a topic from the Topic Table

### Assign Course To Track

```sql
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
```

> [!note] Usage
Assigns a course to a track

### Delete Course From Track

```sql
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
```

> [!danger] Usage
Deletes a course from a track

### Read All Courses For Track

```sql
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
```

> [!tldr] Usage
Reads all courses for a track

### Add Question

```sql
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
```

> [!success] Usage
Adds a question to the Question Table

### Delete Question

```sql
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
```

> [!danger] Usage
Deletes a question from the Question Table

### Update Question

```sql
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
```

> [!warning] Usage
Updates a question in the Question Table

### Read All Questions

```sql
create proc Read_All_Questions
as
begin
	select *
	from Question q
	join Choice c
	on c.ques_id = q.ques_id
end
```

> [!info] Usage
Reads all questions from the Question Table

### Read All Questions For Course By Instructor

```sql
create proc Read_All_Questions_For_Course_By_Instructor @InstructorId varchar(14), @courseId int
as
begin
	begin try
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
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch

end
```

> [!tldr] Usage
Reads all questions for a course by an instructor

### Read All Questions For Course

```sql
create proc Read_All_Questions_For_Course @courseId int
as
begin
	begin try
		select c.crs_name, q.ques_tittle, ch.A, ch.B, ch.C, ch.D, q.ques_answer, q.ques_weight 
		from Question q
		join Course c
		on c.crs_id = q.crs_id
		join Choice ch
		on ch.ques_id = q.ques_id
		where q.crs_id = @courseId;
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch

end
```

> [!tldr] Usage
Reads all questions for a course

### Read All Exams For Course

```sql
create proc Read_All_Exams_For_CourseId @crsId int
as
begin
	begin try
		select e.Ex_id, e.Ex_grade, e.Ex_duration, c.crs_id, c.crs_name
		from Exam e
		join Course c
		on c.crs_id = e.crs_id
		where c.crs_id = @crsId;
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end
```

> [!info] Usage
Reads all exams for a course

### Delete Exam By Id

```sql
create proc Delete_Exam_By_Id @id int
as
begin
	begin try
	if exists(select * from Track_Exam where Exam_id = @id)
		throw 50000, 'Exam is assigned to track', 1;
	else
		delete from Exam where Ex_id = @id;
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end
```

> [!danger] Usage
Deletes an exam by Id

### Assign Exam For Track

```sql
create proc Assign_Exam_For_Track @trackId int, @BranchId int, @ExamId int, @ExamDate dateTime
as
begin
	begin try
		insert into Track_Exam(tr_id, Branch_id, Exam_id, Exam_date)
		values (@trackId, @BranchId, @ExamId, @ExamDate);
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end
```

> [!note] Usage
Assigns an exam for a track

### Read Instructor Courses For Branch and Track

```sql
create proc Read_Instructor_Courses_From_Track_Branch @instructorId varchar(14), @trackId int, @branchId int
as
begin
	begin try
		select c.crs_id, c.crs_name, c.crs_duration
		from Instructor_Teach_Course_For_Track_In_Branch ictb
		join Course c
		on c.crs_id = ictb.crs_id
		where ictb.ins_id = @instructorId and ictb.branch_id = @branchId and ictb.track_id = @trackId;
		if @@ROWCOUNT = 0
			throw 50000, 'No courses found', 1;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end
```

> [!success] Usage
Reads instructor courses for a branch and track

### Read Track For Instructor Course Branch

```sql
create proc Read_Track_From_Instructor_Course_Branch @instructorId varchar(14), @crsId int, @branchId int
as
begin
	begin try
		select t.track_id, t.track_name
		from Instructor_Teach_Course_For_Track_In_Branch ictb
		join Track t
		on t.track_id = ictb.track_id
		where ictb.ins_id = @instructorId and ictb.branch_id = @branchId and ictb.crs_id = @crsId;
		if @@ROWCOUNT = 0
			throw 50000, 'No courses found', 1;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end
```

> [!warning] Usage
Reads track for an instructor course branch

### Read Exam For Student

```sql
create proc Read_Exams_For_Student_Id @studentId varchar(14)
as
begin
	begin try
		select u.ID, u.Name, te.Exam_id, te.Exam_date, e.crs_id, c.crs_name, e.Ex_grade, e.Ex_passGrade
		from Student s
		join Users u
		on u.ID = s.std_id
		join Track_Exam te
		on te.tr_id = s.track_id
		join Exam e
		on e.Ex_id = te.Exam_id
		join Course c
		on c.crs_id = e.crs_id
		where s.std_id = '29803121600573';
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end
```

> [!tldr] Usage
Reads exams for a student

### Read Incoming Exams For Student

```sql
create proc Read_Incoming_Exams_For_Student_Id @studentId varchar(14)
as
begin
	begin try
		select u.ID, u.Name, te.Exam_id, te.Exam_date, e.crs_id, c.crs_name, e.Ex_grade, e.Ex_passGrade
		from Student s
		join Users u
		on u.ID = s.std_id
		join Track_Exam te
		on te.tr_id = s.track_id
		join Exam e
		on e.Ex_id = te.Exam_id
		join Course c
		on c.crs_id = e.crs_id
		where s.std_id = @studentId and te.Exam_date > GETDATE();
		exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end
```

> [!tldr] Usage
Reads incoming exams for a student


### Generate exam with random questions

```sql
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
```

> [!info] Usage
Generate exam with random questions


### model answers for an exam

```sql
	alter proc Exam_Answers @Ex_id int
as
begin
	select q.ques_answer
	from Exam_Question eq,Question q
	where eq.Exam_id=@Ex_id and q.ques_id=eq.Question_id
end
```

> [!info] Usage
model answers for an exam


### Exam Correction

```sql
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
	set @grade = @Student_Grade_Sum/@Grade_Sum*100
	exec Add_Grade_To_Student @std_id,@Ex_id,@grade
	select @grade
end
```

> [!info] Usage
Takes exam id and the answers of a student and corrects the exam

### Read all branches data

```sql
	alter proc Read_All_Branches
as
begin
	select * from Branch
end
```

> [!info] Usage
Read all branches data

### Insert a new branch
```sql
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
```

> [!info] Usage
Insert a new branch


### Update a specific branch data
```sql
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
```

> [!info] Usage
Update a specific branch data through the branch id

### Delete a specific branch
```sql
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
```

> [!info] Usage
Delete a specific branch


### Read all Students data

```sql
	alter proc Read_All_Students
as
begin
	select * from Student
end
```

> [!info] Usage
Read all Students data

### Insert a new student
```sql
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
```

> [!info] Usage
Insert a new student


### Update a specific student data
```sql
	alter proc Update_Student @std_id varchar(14),@std_name varchar(25), @std_password varchar(15),@std_mobile varchar(11),@std_birthDate date,@track_id int,@branch_id int
as
begin
	begin try
		begin transaction
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
```

> [!info] Usage
Update a specific student data through the student id

### Delete a specific student
```sql
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
```

> [!info] Usage
Delete a specific student

### Read all tracks data

```sql
	create proc Read_All_Tracks 
as 
begin 
    select * from Track
end
```

> [!info] Usage
Read all tracks data

### Insert a new track
```sql
	create proc Add_Track 
    @TrackName varchar(25), 
    @SuperId varchar(14) 
as
begin 
    begin try 
        insert into Track(sup_id, track_name) 
        values (@SuperId, @TrackName)
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
```

> [!info] Usage
Insert a new track


### Update a specific track data
```sql
	create proc Update_Track 
    @TrackId int , 
    @TrackName varchar(25), 
    @SuperId varchar(14) 
as
begin 
    begin try
        update Track 
        set track_name = @TrackName, 
            sup_id = @SuperId
        where track_id = @TrackId 
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
```

> [!info] Usage
Update a specific track data through the track id

### Delete a specific track
```sql
	create proc Delete_Track 
    @TrackId int 
as
begin
    begin try
        delete from Track where track_id = @TrackId
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
```

> [!info] Usage
Delete a specific track

### Read all instructor data

```sql
	alter proc Read_All_Instructors
as
begin
    select Ins_id, u.Name, u.Password, u.Mobile 
	from Instructor i
	join Users u
	on u.ID = i.Ins_id
end
```

> [!info] Usage
Read all instructor data

### Insert a new instructor
```sql
	alter proc Add_Instructor 
    @Ins_id varchar(14), 
    @Ins_name varchar(25), 
    @ins_password varchar(15), 
    @Ins_mobile nchar(11)
as
begin 
    begin try 
			begin transaction
				insert into Instructor(Ins_id) 
				values (@Ins_id);
				insert into Users(ID, Name, Password, Mobile, RoleId)
				values (@Ins_id, @Ins_name, @ins_password, @Ins_mobile, 2)
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
```

> [!info] Usage
Insert a new instructor


### Update a specific instructor data
```sql
	alter proc Update_Instructor 
    @Ins_id varchar(14), 
    @Ins_name varchar(25), 
    @ins_password varchar(15), 
    @Ins_mobile nchar(11)
as
begin 
    begin try
		begin transaction
		update Users
		set Name = @Ins_name, Password = @ins_password, Mobile = @Ins_mobile
		where Users.ID = @Ins_id;
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
```

> [!info] Usage
Update a specific instructor data through the instructor id

### Delete a specific instructor
```sql
	alter proc Delete_Instructor 
    @Ins_id varchar(14)
as
begin
    begin try
		begin transaction
        delete from Instructor where Ins_id = @Ins_id;
		delete from Users where ID = @Ins_id;
        if @@ROWCOUNT = 0
            throw 50000, 'Instructor not found', 1;
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
```

> [!info] Usage
Delete a specific instructor

### Add the grade of the exam to the student
```sql
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
```

> [!info] Usage
Add the grade of the exam to the student

---
## Reports Stored Procedures

### Read All students data for Track

```sql
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
```

> [!info] Usage
Reads all students data for a track

### Read All student Grades 

```sql
alter proc Read_Student_Grades_By_Student_Id @studentId int
as
begin
	begin try
		select s.std_name, c.crs_name, se.Exam_id
		from Student_Exam_Grade se
		join Student s
		on s.std_id = se.std_id
		join Track_Exam te
		on te.Exam_id = se.Exam_id
		join Exam e
		on e.Ex_id = te.Exam_id
		join Course c
		on c.crs_id = e.crs_id
		where se.std_id = @studentId
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end
```

> [!tldr] Usage
Reads all student grades in all exams

### Read All instructor Courses

```sql
create proc Read_Instructor_Courses_By_Instructor_Id @instructorId varchar(14)
as
begin
	begin try
		select 
		c.crs_id, c.crs_name, t.track_id, t.track_name, b.BranchId, b.BranchName, count(s.std_id) as 'Number Of Students'
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
		group by c.crs_name, c.crs_id, t.track_id, t.track_name, b.BranchId, b.BranchName
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end
```

> [!success] Usage
Reads all instructor courses

### Read Topics For Course

```sql
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
```

> [!info] Usage
Reads all topics for a course

### Read All Exam Questions
    
```sql

create proc Read_Exam_Questions @ExamId int
as
begin
	begin try
	select q.ques_id, q.ques_tittle, 
	case
		when q.ques_type = 'M' then CONCAT_WS('@@@ ', c.A, c.B, c.C, c.D)
		when q.ques_type = 'T' then CONCAT_WS('@@@ ', 'True', 'False')
	end as 'Choices',
		case 
		when q.ques_type = 'T' then
		case
		when q.ques_answer = 'A'
			then 'True'
		else
			'False'
		end
	when q.ques_type = 'M' then
		case 
		when q.ques_answer = 'A'
			then c.A
		when q.ques_answer = 'B'
			then c.B
		when q.ques_answer = 'C'
			then c.C
		else
			c.D
		end
	end as 'Model Answer'
	from Exam_Question eq
	join Question q
	on q.ques_id = eq.Question_id
	left join Choice c
	on c.ques_id = q.ques_id
	where eq.Exam_id = @ExamId;
	exec Throw_Error_No_Rows_Affected;
	end try
	begin catch
		exec Show_Error;
		exec Log_Error;
	end catch
end
```

> [!tldr] Usage
Reads all exam questions

### Read Questions with Student's Answers

```sql
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
```

> [!info] Usage
Reads questions with student's answers for an exam.

---
