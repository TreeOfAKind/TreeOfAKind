import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './authentication/login/login.component';
import { RegisterComponent } from './authentication/register/register.component';
import { AuthGuard } from './authentication/shared/auth.guard'
import { HomeComponent } from './home/home.component';
import { UserProfileFormComponent } from './user-profile/user-profile-form/user-profile-form.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', canActivate: [AuthGuard], children: [
    { path: '', component: HomeComponent },
    { path: 'user-profile', component: UserProfileFormComponent }
  ]},
  { path: '*', redirectTo: ''},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
