class User{
    constructor(_id,_password,_role)
    {
        this.Id = _id;
        this.Password = _password;
        this.Role = _role;
    }

    toString()
    {
        return `${this.Id} : ${this.Password} : ${this.Role}`;
    }
}


class Student extends User {
    constructor(_id,_password,_role,_name,_mobileNo,_birthDate)
    {
        super(_id,_password,_role)
        this.Name = _name;
        this.MobileNo = _mobileNo;
        this.BirthDate = _birthDate;
    }

    toSting()
    {
        return `${super.toString()} : ${this.Name} : ${this.MobileNo} : ${this.BirthDate}`
    }
}

let std = new Student(10,"a123456789","ahmed","01141600918","15-4-2020");
console.log(std.Name);

