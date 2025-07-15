import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, type Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { APP_CONFIG } from '../configs/environment.config';
import { User } from '../interfaces/user.interface';
import { API_ENDPOINTS } from '../constants/api.constants';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  apiUrl = APP_CONFIG.apiBaseUrl;
  imageUrl = APP_CONFIG.imageBaseUrl;

  currentUser: BehaviorSubject<any> = new BehaviorSubject(null);
  currentUserName: BehaviorSubject<any> = new BehaviorSubject(null);
  currentUserData: any;
  public userPayLoad: any;
  jwthelperService = new JwtHelperService();

  constructor(public http: HttpClient) {
   // this.userPayLoad = this.decodedToken();
  }

  registerUser(user: User) {
    return this.http.post(
      `${this.apiUrl}${API_ENDPOINTS.AUTH.REGISTER}`,
      user,
      { responseType: 'json' }
    );
  }

  getUserById(id: number): Observable<User[]> {
    return this.http.get<User[]>(
      `${this.apiUrl}${API_ENDPOINTS.AUTH.GET_USER_BY_ID}/${id}`
    );
  }

  updateUser(data: FormData) {
  return this.http.post(
  `${this.apiUrl}${API_ENDPOINTS.AUTH.UPDATE_USER}`,
  data
  );

  }
  

  loginUser(loginInfo: Array<string>) {
    return this.http.post(
      `${this.apiUrl}${API_ENDPOINTS.AUTH.LOGIN}`,
      {
        emailAddress: loginInfo[0],
        password: loginInfo[1],
      },
      { responseType: 'json' }
    );
  }

  forgotPasswordEmailCheck(data: any) {
    return this.http.post(
      `${this.apiUrl}${API_ENDPOINTS.AUTH.FORGOT_PASSWORD}`,
      data
    );
  }

  resetPassword(data: any) {
    return this.http.post(
      `${this.apiUrl}${API_ENDPOINTS.AUTH.RESET_PASSWORD}`,
      data,
      { responseType: 'text' }
    );
  }

  changePassword(data: any) {
    return this.http.post(
      `${this.apiUrl}${API_ENDPOINTS.AUTH.CHANGE_PASSWORD}`,
      data
    );
  }

  getToken() {
   // console.log(localStorage.getItem('access_Token'));
    return localStorage.getItem("access_Token");
  }

  setToken(token: string) {
  //console.log(localStorage.getItem('access_Token'));
  localStorage.setItem('access_Token', token);
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('access_Token') ? true : false;
  }

  loggedOut() {
    localStorage.removeItem('access_Token');
  }
  public getCurrentUser() {
    return this.currentUser.asObservable();
  }

  public setCurrentUser(userDetail: any) {
    return this.currentUser.next(userDetail);
  }

  decodedToken() {
    const token = this.getToken();
    if (!token) return null;
    return this.jwthelperService.decodeToken(token);
  }

  getUserFullName() {
  const user = this.getUserDetail();
  return user?.fullName ?? "Unknown";
  }

  public getUserDetail() {
  const token = this.getToken();
  return token ? this.jwthelperService.decodeToken(token) : null;
  }

}
