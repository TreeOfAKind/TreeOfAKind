import { Component, OnInit } from '@angular/core';
import { AuthService } from '../authentication/shared/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  navbarOpen: boolean = false;
  public email: string = 'dupa';

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.email = this.authService.user.email;
  }

  toggleNavbar() {
    this.navbarOpen = !this.navbarOpen;
  }
}
