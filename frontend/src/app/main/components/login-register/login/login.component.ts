import { NgIf } from "@angular/common"
import { Component, OnDestroy, OnInit } from "@angular/core"
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms"
import { Router, RouterModule } from "@angular/router"
import { NgToastService } from "ng-angular-popup"
import { Subscription } from "rxjs"
import { APP_CONFIG } from "src/app/main/configs/environment.config"
import { AuthService } from "src/app/main/services/auth.service"

@Component({
  selector: "app-login",
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, RouterModule],
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent implements OnInit, OnDestroy {
  private unsubscribe: Subscription[] = [];
  
  constructor(
    private _fb: FormBuilder,
    private _service: AuthService,
    private _router: Router,
    private _toast: NgToastService,
  ) { }
  loginForm: FormGroup
  formValid: boolean
  ngOnInit(): void {
    this.loginUser()
  }
  loginUser() {
    this.loginForm = this._fb.group({
      emailAddress: [null, Validators.compose([Validators.required, Validators.email])],
      password: [null, Validators.compose([Validators.required])],
    })
  }
  get emailAddress() {
    return this.loginForm.get("emailAddress") as FormControl
  }
  get password() {
    return this.loginForm.get("password") as FormControl
  }
 onSubmit() {
  this.formValid = true;

  if (this.loginForm?.valid) {
    const loginUserSubscribe = this._service
      .loginUser([
        this.loginForm.value.emailAddress,
        this.loginForm.value.password
      ])
      .subscribe({
        next: (res: any) => {
          if (res.success && res.data?.token) {
            this._service.setToken(res.data.token);

            const tokenPayload = this._service.decodedToken();
            const role = tokenPayload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

            this._service.setCurrentUser(tokenPayload);

            this._toast.success({
              detail: "SUCCESS",
              summary: `You have entered as ${role}`,
              duration: APP_CONFIG.toastDuration
            });

            if (role === "admin") {
              this._router.navigate(["admin/dashboard"]);
            } else {
              this._router.navigate(["/home"]);
            }
          } else {
            this._toast.error({
              detail: "ERROR",
              summary: res.message || "Invalid login",
              duration: APP_CONFIG.toastDuration
            });
          }
        },
        error: (err) => {
          this._toast.error({
            detail: "ERROR",
            summary: err.error?.message || "Server error",
            duration: APP_CONFIG.toastDuration
          });
        }
      });
      this.formValid = false
      this.unsubscribe.push(loginUserSubscribe);
    }
  }
  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe())
  }
}
