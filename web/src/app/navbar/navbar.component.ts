import { Component, OnInit } from '@angular/core';
import { AuthService } from '../authentication/shared/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  navbarOpen: boolean = false;

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit(): void {

  }

  toggleNavbar() {
    this.navbarOpen = !this.navbarOpen;
  }

  logout() {
    this.authService.logout();
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  getUserEmail() {
    return this.authService.getUser().email;
  }
}
