import {
  Component,
  OnInit,
  ChangeDetectorRef,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UserService } from '../../services/user';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './dashboard.html',
})
export class DashboardComponent implements OnInit {
  private userService = inject(UserService);
  private cdr = inject(ChangeDetectorRef);
  piechart: any;
  barchart: any;
  users: any[] = [];
  filteredUsers: any[] = [];

  loading = false;
  error = '';

  filters = {
    gender: '',
    department: '',
    state: ''
  };

  ngOnInit() {
    this.loadAllUsers();
  }

  chartHandler(usersData : any){
    const femaleCount = usersData.filter((user:any) => user.gender.toLowerCase() =="female").length;
    const maleCount = usersData.filter((user:any) => user.gender.toLowerCase() == "male").length;
    const maleFemaleCnt: number[] = [maleCount, femaleCount];
    const roleMap = new Map();

    usersData.forEach((user:any) => {
      const role = user.role;
      if (roleMap.has(role)) {
        roleMap.set(role, roleMap.get(role) + 1);
      } else {
        roleMap.set(role, 1);
      }
    });

    const roles = [...roleMap.keys()];
    const counts = [...roleMap.values()];
    const barchartdata = {
    labels: roles,
    datasets: [{
        label: 'Roles Analysis',
        data: counts,
        backgroundColor: [
          'rgb(180, 116, 130)',
          'rgb(0, 255, 255)',
          'rgb(111, 31, 191)'
        ],
        borderColor: [
          'rgb(255, 99, 132)',
          'rgb(255, 159, 64)',
          'rgb(255, 205, 86)',
          'rgb(75, 192, 192)',
          'rgb(54, 162, 235)',
          'rgb(153, 102, 255)',
          'rgb(201, 203, 207)'
        ],
        borderWidth: 1
      }]
    };
    this.createChart(maleFemaleCnt,barchartdata);
  }

  createChart(maleFemaleCnt:number[], barchartData:any){
      this.piechart = new Chart("MyPieChart", {
        type: 'pie', 
        data: {
          labels: ['Male','Female'],
            datasets: [{
                label: 'Male Female Ratio',
                data: maleFemaleCnt,
                backgroundColor: [
                  'blue',            
                  'pink'
                ],
                hoverOffset: 4
              }],
        },
        options: {
          aspectRatio:2.5
        }

      });
      this.barchart = new Chart("MyBarChart", {
        type: 'bar', 
        data: barchartData,
        options: {
          aspectRatio:2.5
        }
      });
  }

  trackByUserId( user: any): number {
    return user.id;
  }

  private loadAllUsers() {
    this.loading = true;
    this.userService.getUsers(0,30).subscribe({ 
      next: (res) => {
        this.users = res.users || [];
        this.filteredUsers = [...this.users];
        this.loading = false;
        this.chartHandler(this.users)
        console.log(this.users)
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Failed to load users. Please try again later.';
        this.loading = false;
      }
    });
  }

  applyFilters() {
    this.updateFilteredUsers();
  }

  private updateFilteredUsers() {
    this.filteredUsers = this.applyCurrentFilters(this.users);
  }

  private applyCurrentFilters(users: any[]) {
    return users.filter(user => {
      const genderMatch = this.filters.gender ? user.gender === this.filters.gender : true;
      const departmentMatch = this.filters.department ? user.company.department === this.filters.department : true;
      const stateMatch = this.filters.state ? user.address.state === this.filters.state : true;
      return genderMatch && departmentMatch && stateMatch;
    });
  }

  getUniqueValues(key: string): string[] {
    const values = new Set<string>();
    this.users.forEach(user => {
      let value = '';
      if (key === 'department') value = user.company.department;
      else if (key === 'state') value = user.address.state;
      else value = user[key];
      if (value) values.add(value);
    });
    return Array.from(values).sort();
  }

  get visibleUsers() {
    return this.filteredUsers;
  }
}
