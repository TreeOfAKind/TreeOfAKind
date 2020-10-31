import { Injectable, NgZone } from '@angular/core';
import { Router } from  "@angular/router";
import { AngularFireAuth } from  "@angular/fire/auth";
import { renderFlagCheckIfStmt } from '@angular/compiler/src/render3/view/template';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  user = {
    email: null
  }

  constructor(
    private afAuth: AngularFireAuth,
    private router: Router,
    private zone: NgZone
  ) {
    this.afAuth.authState.subscribe(user => {
      if (user) {
        this.user.email = user.email;
        localStorage.setItem('user', JSON.stringify(this.user));
        JSON.parse(localStorage.getItem('user'));
      } else {
        this.user = null;
        localStorage.setItem('user', null);
        JSON.parse(localStorage.getItem('user'));
      }
    })
   }

  async register(email: string, password: string) {
    await this.afAuth.createUserWithEmailAndPassword(email, password)
      .then(result => {
        this.zone.run(() => this.router.navigate(['/login']));
      })
      .catch(error => {
        window.alert(error.message);
      });
  }

  async login(email: string, password: string) {
    return this.afAuth.signInWithEmailAndPassword(email, password)
      .then((result) => {
        this.zone.run(() => {
          this.router.navigate(['']);
        });
        this.setUserData(result.user);
      }).catch((error) => {
        window.alert(error.message)
      })
  }

  getUser() {
    const user = JSON.parse(localStorage.getItem('user'));
    return user;
  }

  isLoggedIn() {
    const user = this.getUser();
    return user != null;
  }

  private setUserData(user) {
    this.user = { email: user.email };
    localStorage.setItem('user', JSON.stringify(this.user));
  }
}
