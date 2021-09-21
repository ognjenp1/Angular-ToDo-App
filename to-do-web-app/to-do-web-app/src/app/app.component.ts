import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NavbarService } from './core/navbar.service';
import { SearchService } from './core/search.service';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title: string = 'to-do-web-app';
  userName?: string;

  constructor(
    private router: Router,
    public nav: NavbarService,
    private searchService: SearchService,
    public auth: AuthService
  ) {
    this.nav.show();
    this.auth.isAuthenticated$.subscribe((data) => {
      if (!data) {
        this.auth.loginWithRedirect();
      } else {
        this.auth.user$.subscribe((token) => {
          this.userName = token?.nickname;
        });
      }
    });
  }

  search(event: any) {
    this.searchService.sendSearchText(event.target.value);
  }

  showSearchBox() {
    this.nav.show();
  }

  hideSearchBox() {
    this.nav.hide();
  }

  btnClickAdd() {
    this.router.navigateByUrl('/to-do-lists');
  }
}
