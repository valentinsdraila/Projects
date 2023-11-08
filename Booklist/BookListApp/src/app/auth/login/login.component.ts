import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { AuthServiceService } from '../auth-service.service';
import { CookieService } from '../cookieService';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit {
  logInForm!: FormGroup;

  constructor(private router: Router, private _snackBar: MatSnackBar, private http: HttpClient,
    private service: AuthServiceService, private cookieService : CookieService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.logInForm = new FormGroup({
      email: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required),
    });
  }

  logIn() {
    //todo 
    
    // const body = {
    //   email: this.email?.value,
    //   password: this.password?.value
    // }

    // this.http.post("http://localhost:44340/api/users/login", body).subscribe(
    //   (res: any) => {
    //     console.log(res);
    //     this.router.navigateByUrl('main-page');
    //     this._snackBar.open('Log In Successfully!', '', {
    //       duration: 2000,
    //     });
    //     window.localStorage.setItem("token",res.token)
    //   },
    //   (error) => {
    //     console.error(error);
    //     this._snackBar.open(error.error.error, '', {
    //       duration: 2000,
    //     });
    //   }
    // )

    let email: string = this.email?.value;
    let password: string = this.password?.value;

    const body = {
      email: email,
      password: password
    }

    this.http.post("http://localhost:44340/api/users/login", body).subscribe(
      (res:any) => {
        console.log(res)
        this.router.navigate(['main-page'])
        this._snackBar.open('Log in Successfully!', '', {
          duration: 2000,
        })
        window.localStorage.setItem("token", res.token)
      },(error) => {
        console.error("error");
        this._snackBar.open("There is no account with these credentials", '', {
          duration: 2000,
        });
      }
    )    
  }

  rememberMe(ob: MatCheckboxChange) {
    //to do
 } 

  get email() {
    return this.logInForm.get('email');
  }

  get password() {
    return this.logInForm.get('password');
  }

}
