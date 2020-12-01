import { Component, OnInit } from '@angular/core';
import { UserProfile } from '../shared/user-profile.model'
import { UserProfileService } from '../shared/user-profile.service'

@Component({
  selector: 'app-user-profile-form',
  templateUrl: './user-profile-form.component.html',
  styleUrls: ['./user-profile-form.component.scss']
})
export class UserProfileFormComponent implements OnInit {
  model: UserProfile = {
    firstName: null,
    lastName: null,
    birthDate: null
  };

  constructor(
    private service: UserProfileService
  ) { }

  ngOnInit(): void {
    this.service.getUserProfile().subscribe(result => {
      if(result != null) {
        this.model = result;
      }
    })
  }

  onSubmit() {
    this.service.updateUserProfile(this.model).subscribe();
  }

}
