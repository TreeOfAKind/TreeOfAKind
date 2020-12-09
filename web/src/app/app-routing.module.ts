import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './authentication/login/login.component';
import { RegisterComponent } from './authentication/register/register.component';
import { AuthGuard } from './authentication/shared/auth.guard'
import { TreesListComponent } from './tree/trees-list/trees-list.component';
import { UserProfileFormComponent } from './user-profile/user-profile-form/user-profile-form.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', canActivate: [AuthGuard], children: [
    { path: '', component: TreesListComponent },
    { path: 'user-profile', component: UserProfileFormComponent },
    { path: 'trees-list', component: TreesListComponent },
  ]},
  { path: '*', redirectTo: ''},
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
