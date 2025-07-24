import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Navigation } from '../../../shared/navigation/navigation';
import { TesterService } from '../../../services/TesterService';
import { BugSubmissionDTO, BugPriority, BugResponse } from '../../../models/bug.model';

@Component({
  selector: 'app-create-bug',
  imports: [CommonModule, FormsModule, Navigation],
  templateUrl: './create-bug.html',
  styleUrl: './create-bug.css'
})
export class CreateBug implements OnInit{
  bugs: BugResponse[] = [];
  parentBugInput: string = '';
  bug: BugSubmissionDTO = {
    title: '',
    description: '',
    priority: BugPriority.Medium,
    screenshotUrl: undefined,
    parentBugs: []
  };

  selectedFile: File | null = null;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private testerService: TesterService,
    private router: Router
  ) {}

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  onSubmit(): void {
    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    if (this.selectedFile) {
      this.testerService.uploadScreenshot(this.selectedFile).subscribe({
        next: (response) => {
          this.bug.screenshotUrl = response.url || response.fileUrl;
          this.createBug();
        },
        error: (error) => {
          console.warn('Screenshot upload failed, proceeding without it:', error);
          this.createBug();
        }
      });
    } else {
      this.createBug();
    }
  }

    ngOnInit(): void {
      this.loadMyBugs();
    }
  
    loadMyBugs(): void {
      this.testerService.getMyBugs().subscribe({
        next: (response : any) => {
        const values = response?.$values || [];
  
        this.bugs = values.map((bug: any) => {
          return {
            id: bug.id,
            title: bug.title,
            description: bug.description,
            screenshotUrl: bug.screenshotUrl,
            priority: bug.priority,
            status: bug.status,
            createdBy: bug.createdBy,
            assignedTo: bug.assignedTo,
            isDeleted: bug.isDeleted,
            createdAt: bug.createdAt,
            updatedAt: bug.updatedAt,
            parentBugIds: bug.parentBugIds?.$values || [] 
          } as BugResponse;
        });
        console.log(this.bugs);
  
        },
        error: (error) => {
        console.error('Error loading your bugs:', error);
      }
      });
    }
  private createBug(): void {
    this.bug.parentBugs = this.parentBugInput
      .split(',')
      .map(id => parseInt(id.trim(), 10))
      .filter(id => !isNaN(id));
    this.testerService.createBug(this.bug).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.successMessage = 'Bug created successfully!';
        setTimeout(() => {
          this.router.navigate(['/tester/bugs']);
        }, 2000);
      },
      error: (error) => {
        this.isSubmitting = false;
        this.errorMessage = error.error?.message || 'Failed to create bug. Please try again.';
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/tester/dashboard']);
  }
}

