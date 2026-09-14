import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  username: string = 'User';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Agar AuthService mein current user ya email lene ka method ya property hai toh yahan use karein
    // Misal ke tor par agar localStorage se name milta hai ya authService se:
    const currentUser = this.authService.getCurrentUser(); // ya jo bhi method aapke auth service mein ho
    if (currentUser && currentUser.username) {
      this.username = currentUser.username;
    } else {
      // Fallback agar email save ho ya token se decode karna ho
      this.username = localStorage.getItem('username') || 'User';
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}