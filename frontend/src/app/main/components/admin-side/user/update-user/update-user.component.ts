import { Component, OnDestroy, ViewChild, type OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/main/services/auth.service';
import { APP_CONFIG } from 'src/app/main/configs/environment.config';
import { HeaderComponent } from '../../header/header.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { NgIf } from '@angular/common';
import { Subscription } from 'rxjs';
import { ClientService } from 'src/app/main/services/client.service';
import { Role } from 'src/app/main/enums/roles.enum';

@Component({
  selector: 'app-update-user',
  standalone: true,
  imports: [SidebarComponent, HeaderComponent, ReactiveFormsModule, NgIf],
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.css'],
})
export class UpdateUserComponent implements OnInit, OnDestroy {
  private unsubscribe: Subscription[] = [];

  updateForm: FormGroup;
  formValid: boolean;
  userId: string; // Store the user ID
  updateData: any;
  isupdateProfile: boolean;
  currentLoggedInUser: any;
  headText: string = 'Update User';
  userImage: any = '';
  selectedFile: File;
  previewUrl: string | ArrayBuffer;
  @ViewChild('imageInput') imageInputRef: any;

  constructor(
    private _fb: FormBuilder,
    private _service: AuthService,
    private _clientService: ClientService,
    private _toastr: ToastrService,
    private _activateRoute: ActivatedRoute,
    private _router: Router,
    private _toast: NgToastService
  ) {}

  ngOnInit(): void {
    // Extract user ID from route params
    this.userId = this._activateRoute.snapshot.paramMap.get('userId');
    console.log("UpdateUserComponent loaded. User ID:", this.userId);

    // Initialize updateForm with validators
    this.updateForm = this._fb.group({
      id: [this.userId || ''],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: [
        '',
        [Validators.required, Validators.minLength(10), Validators.maxLength(10)],
      ],
      emailAddress: ['', [Validators.required, Validators.email]],
    });

    // Check URL to set profile update mode
    const url = this._router.url;
    if (url.includes('updateProfile')) {
      this.isupdateProfile = true;
      this.headText = 'Update Profile';
    } else {
      this.isupdateProfile = false;
      this.headText = 'Update User';
    }

    // Get current logged-in user from token
    this.currentLoggedInUser = this._service.getUserDetail();
    console.log("Current Logged In User:", this.currentLoggedInUser);

    // Authorization check
    if (this.userId && this.currentLoggedInUser) {
      const currentRole = this.currentLoggedInUser.userType || this.currentLoggedInUser["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      const currentUserId = this.currentLoggedInUser.userId;

      console.log("Current User Role:", currentRole);
      console.log("Current User ID:", currentUserId);

      if (currentRole && currentRole.trim().toLowerCase() !== "admin") {
        if (this.userId !== currentUserId) {
          this._toast.error({
            detail: 'ERROR',
            summary: 'You are not authorized to access this page',
            duration: APP_CONFIG.toastDuration,
          });
          this._router.navigate(['/home']);
          return;
        }
      }

      // Fetch user details for the given userId
      this.fetchDetail(this.userId);
    } else {
      console.log("❌ userId or currentLoggedInUser not found. Redirecting to /home.");
      this._router.navigate(['/home']);
    }
  }

  get firstName() {
    return this.updateForm.get('firstName') as FormControl;
  }
  get lastName() {
    return this.updateForm.get('lastName') as FormControl;
  }
  get phoneNumber() {
    return this.updateForm.get('phoneNumber') as FormControl;
  }
  get emailAddress() {
    return this.updateForm.get('emailAddress') as FormControl;
  }

  fetchDetail(id: any) {
    const getUserSubscribe = this._clientService
      .loginUserDetailById(id)
      .subscribe((data: any) => {
        this.updateData = data.data;
        this.updateForm = this._fb.group({
          id: [this.updateData.id],
          firstName: [this.updateData.firstName, Validators.required],
          lastName: [this.updateData.lastName, Validators.required],
          phoneNumber: [
            this.updateData.phoneNumber,
            [Validators.required, Validators.minLength(10), Validators.maxLength(10)],
          ],
          emailAddress: [
            {
              value: this.updateData.emailAddress,
              disabled: this.isupdateProfile,
            },
            [Validators.required, Validators.email],
          ],
          userType: [this.updateData.userType],
        });
      });
    this.unsubscribe.push(getUserSubscribe);
  }

  onSubmit() {
    this.formValid = true;
    if (this.updateForm.valid) {
      const formData = new FormData();
      const updatedUserData = this.updateForm.getRawValue();
      Object.keys(updatedUserData).forEach((key) => {
        formData.append(key, updatedUserData[key]);
      });

      if (this.selectedFile) {
        formData.append('profileImage', this.selectedFile);
      }

      const updateUserSubscribe = this._service.updateUser(formData).subscribe(
        (data: any) => {
          if (data.status==='Success') {
            this._toast.success({
              detail: 'SUCCESS',
              summary: this.isupdateProfile ? 'Profile Updated Successfully' : 'Profile Updated',
              duration: APP_CONFIG.toastDuration,
            });
            setTimeout(() => {
              if (this.isupdateProfile) {
                this._router.navigate(['admin/profile']);
              } else {
                this._router.navigate(['admin/user']);
              }
            }, 5000);
          } else {
            this._toastr.error(data.message);
          }
        },
        (err) =>
          this._toast.error({
            detail: 'ERROR',
            summary: err.message,
            duration: APP_CONFIG.toastDuration,
          })
      );
      this.formValid = false;
      this.unsubscribe.push(updateUserSubscribe);
    }
  }

  onCancel() {
    if (this.isupdateProfile) {
      this._router.navigate(['admin/profile']);
    } else {
      this._router.navigateByUrl('admin/user');
    }
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      // Preview
      const reader = new FileReader();
      reader.onload = () => (this.previewUrl = reader.result);
      reader.readAsDataURL(file);
    }
  }

  getFullImageUrl(imagePath: string): string {
    return imagePath ? `${APP_CONFIG.imageBaseUrl}/${imagePath}` : '';
  }

  triggerImageInput(): void {
    this.imageInputRef.nativeElement.click();
  }

  cancelImageChange(): void {
    this.selectedFile = null;
    this.previewUrl = null;
    this.updateData.profileImage = null;
  }

  onImageError(event: any): void {
    event.target.src = 'assets/Images/default-user.png';
  }

  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }
}
