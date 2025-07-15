import { Injectable } from "@angular/core";
import {
  CanActivate,
  CanActivateChild,
  Router,
} from "@angular/router";
import { NgToastService } from "ng-angular-popup";
import { AuthService } from "../services/auth.service";
import { APP_CONFIG } from "../configs/environment.config";

@Injectable({
  providedIn: "root",
})
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(
    private service: AuthService,
    private router: Router,
    private toastr: NgToastService
  ) {}

  canActivate(): boolean {
    return this.checkUser();
  }

  canActivateChild(): boolean {
    return this.checkUser();
  }
 private checkUser(): boolean {
  console.log("checkUser function called");

  const tokenPayload = this.service.decodedToken();
  console.log("Full Decoded Token:", tokenPayload);

  let roleClaim = tokenPayload?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
               || tokenPayload?.["role"];

  console.log("Role Claim Type:", typeof roleClaim);
  console.log("Role Claim Exact Value:", JSON.stringify(roleClaim));

  // Final strict check
  if (roleClaim && typeof roleClaim === 'string' && roleClaim.trim().toLowerCase() === "admin") {
    console.log("✅ Role is admin, allowing access");
    return true;
  }

  console.log("❌ Role is not admin, blocking access");
   this.toastr.error({
        detail: "ERROR",
        summary: "You are not authorized to access this page",
        duration: APP_CONFIG.toastDuration,
      });

  this.router.navigate(["/home"]);
  return false;
}

}