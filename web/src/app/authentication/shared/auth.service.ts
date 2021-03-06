import { Injectable, NgZone } from '@angular/core';
import { Router } from  "@angular/router";
import { AngularFireAuth } from  "@angular/fire/auth";
import { User } from './user.model';
import { from, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  user: User;

  constructor(
    private afAuth: AngularFireAuth,
    private router: Router,
    private zone: NgZone
  ) { }

  async register(email: string, password: string) {
    await this.afAuth.createUserWithEmailAndPassword(email, password)
      .then((result) => {
        this.setUserData(result.user);
        this.zone.run(() => this.router.navigate(['/user-profile']));
      })
      .catch(error => {
        window.alert(error.message);
      });
  }

  async login(email: string, password: string) {
    return this.afAuth.signInWithEmailAndPassword(email, password)
      .then((result) => {
        this.setUserData(result.user);
        this.zone.run(() => {
          this.router.navigate(['']);
        });
      }).catch((error) => {
        window.alert(error.message)
      })
  }

  logout() {
    this.afAuth.signOut().then(() => {
      localStorage.removeItem('user');
      this.router.navigate(['login']);
    })
  }

  getUser() {
    const user = JSON.parse(localStorage.getItem('user'));
    return user;
  }

  getToken(): Observable<string> {
    return from(this.afAuth.idToken);
  }

  isLoggedIn() {
    const user = this.getUser();
    return user != null;
  }

  resetPassword(email: string) {
    this.afAuth.sendPasswordResetEmail(email)
      .then()
      .catch((error) => {
        window.alert(error.message);
      });
  }

  private setUserData(user) {
    this.user = { email: user.email };
    localStorage.setItem('user', JSON.stringify(this.user));
  }
}
