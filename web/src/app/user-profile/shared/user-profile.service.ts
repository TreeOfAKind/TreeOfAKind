import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserProfile } from './user-profile.model';
import { environment } from '../../../environments/environment'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {
  url = `${environment.apiUrl}UserProfile`;
  constructor(
    private httpClient: HttpClient
  ) { }

  updateUserProfile(userProfile: UserProfile) {
    return this.httpClient.post(`${this.url}/CreateOrUpdateUserProfile`, userProfile);
  }

  getUserProfile(): Observable<UserProfile> {
    return this.httpClient.post<UserProfile>(`${this.url}/GetMyUserProfile`, {});
  }
}
