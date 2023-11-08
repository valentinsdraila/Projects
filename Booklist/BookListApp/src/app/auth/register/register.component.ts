import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { UserDTO } from '../userDTO';
import { AuthServiceService } from '../auth-service.service';
import { CustomValidators } from '../custom-validators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  hide=true;

  constructor(private router: Router, private _snackBar: MatSnackBar, private http: HttpClient,
    private service: AuthServiceService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = new FormGroup({
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      email: new FormControl(null, [CustomValidators.emailValidator, Validators.required]),
      password: new FormControl(null, [CustomValidators.passwordValidator, Validators.required]),
      confirmedPassword: new FormControl(null, Validators.required)
    });
  }

  postUser() {
    let user = new UserDTO(this.getFirstName?.value, this.getLastName?.value, this.getEmail?.value,
      this.getPassword?.value);

    if (!this.getEmail?.valid)
    {
      this._snackBar.open("Invalid E-mail!", '', {
        duration: 2000,
      });
     return;
    }

    if (!this.getPassword?.valid)
    {
      this._snackBar.open("Invalid Password!", '', {
        duration: 2000,
      });
     return;
    }

    let confirmPassword : Boolean= this.checkPassword(user.password);
    if (confirmPassword == true)
    {
      this._snackBar.open("Register successfully!", '', {
        duration: 2000,
      });
      this.service.postUser(user).subscribe();
    }
    
  }


  checkPassword(password : string) : boolean
  {
    // password is not at least 6 characters long
    if (password.length < 6)
    {
      this._snackBar.open("Password must be at least 6 characters long!", '', {
        duration: 2000,
      });
      return false;
    }

    if (password == this.getConfirmedPassword?.value)
        return true;

    // password not equal to confirmedPassword
    this._snackBar.open("Confirmed password is not equal to initial password!", '', {
      duration: 2000,
    });
    return false;
  }

  get getEmail() {
    return this.registerForm.get('email');
  }

  get getPassword() {
    return this.registerForm.get('password');
  }

  get getLastName() {
    return this.registerForm.get('lastName');
  }

  get getFirstName() {
    return this.registerForm.get('firstName');
  }

  get getConfirmedPassword() {
    return this.registerForm.get('confirmedPassword');
  }
}
