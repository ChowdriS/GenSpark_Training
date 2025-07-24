
export enum BugPriority {
  Low = 0,
  Medium = 1,
  High = 2,
  Critical = 3
}

export enum BugStatus {
  New = 0,
  Assigned = 1,
  InProgress = 2,
  Fixed = 3,
  Retesting = 4,
  Verified = 5,
  Reopened = 6,
  Closed = 7
}
export interface Bug {
  id: number;
  title: string;
  description: string;
  priority: BugPriority;
  status: BugStatus;
  assignedTo?: string;
  createdBy: string;
  isDeleted: boolean;
  createdAt: string;
  updatedAt?: string;
  screenshotUrl?: string;
}

export interface BugResponse {
  id: number;
  title: string;
  description: string;
  priority: BugPriority;
  status: BugStatus;
  assignedTo?: string;
  createdBy: string;
  isDeleted: boolean;
  createdAt: string;
  updatedAt?: string;
  screenshotUrl?: string;
  parentBugIds: number[];
}


export interface BugSubmissionDTO {
  title: string;
  description: string;
  priority: BugPriority;
  screenshotUrl?: string;
  parentBugs : number[];
}

export interface UpdateBugPatchDTO {
  description?: string;
  priority: BugPriority;
  screenshotUrl?: string;
}

export interface BugStats {
  totalBugs: number;
  openBugs: number;
  closedBugs: number;
  inProgressBugs: number;
  resolvedBugs: number;
}
