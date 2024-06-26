-- Track stored procedures

-- Read_All_Tracks
create proc Read_All_Tracks 
as 
begin 
    select * from Track
end

go

-- Add_Track
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

go

-- Update_Track
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

go

-- Delete_Track
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

-- Instructor stored procedures

go

-- Read_All_Instructors
alter proc Read_All_Instructors
as
begin
    select Ins_id, u.Name, u.Password, u.Mobile 
	from Instructor i
	join Users u
	on u.ID = i.Ins_id
end

go

-- Add_Instructor
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

go

-- Update_Instructor
alter proc Update_Instructor 
    @Ins_id varchar(14), 
    @Ins_name varchar(25), 
    @ins_password varchar(15), 
    @Ins_mobile nchar(11)
as
begin 
    begin try
		begin transaction
        --update Instructor 
        --set Ins_name = @Ins_name, 
        --    ins_password = @ins_password, 
        --    Ins_mobile = @Ins_mobile
        --where Ins_id = @Ins_id ;
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

go

-- Delete_Instructor
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
