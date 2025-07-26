import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../services/User/user-service';
import { User } from '../../models/user.model';
import { ApiResponse } from '../../models/api-response.model';
import { Getrole } from '../../misc/Token';
import { Auth } from '../../services/Auth/auth';
import { NotificationService } from '../../services/Notification/notification-service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
  standalone : true
})
export class Navbar implements OnInit {
  user = signal<User | null>(null);
  menuopen = signal<boolean>(false);
  // role :string = "";
  constructor(public router: Router, private userService: UserService, private auth : Auth,  private notify: NotificationService) {}

  logout() {
    this.menuopen.set(false);
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
  menuOpen(){
    this.menuopen.set(!this.menuopen());
  }
  ngOnInit(): void {
    this.getMyDetail();
    // this.role = Getrole(this.auth.getToken());
  }
  navigateToProfile(event : Event) {
    event?.preventDefault();
    this.menuopen.set(false);
    const token = this.auth.getToken();
    const role = Getrole(token);
    const lowerrole = role.toLowerCase();
    const route = `/${lowerrole}/profile`;
    this.router.navigate([route]);
  }
  getMyDetail() {
    this.userService.getUserDetails().subscribe({
      next: (res: ApiResponse) => {
        this.user.set(res.data);
      },
      error: (err: any) => {
        this.notify.error("Failed to fetch your Data");
      }
    });
  }
}
