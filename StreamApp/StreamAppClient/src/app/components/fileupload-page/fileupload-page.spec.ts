import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FileuploadPage } from './fileupload-page';

describe('FileuploadPage', () => {
  let component: FileuploadPage;
  let fixture: ComponentFixture<FileuploadPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FileuploadPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FileuploadPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
