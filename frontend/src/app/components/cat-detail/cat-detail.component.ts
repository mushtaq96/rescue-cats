import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CatService } from '../../services/cat.service';
import { AuthService } from '../../services/auth.service';
import { Cat, AdoptionApplication } from '../../models/cat.model';

@Component({
  selector: 'app-cat-detail',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './cat-detail.html'
})
export class CatDetailComponent implements OnInit {
  cat?: Cat;
  adoptionForm: FormGroup;
  isSubmitting = false;
  showSuccess = false;
  hasApplied = false;

  constructor(
    private route: ActivatedRoute,
    private catService: CatService,
    private fb: FormBuilder,
    private authService: AuthService
  ) {
    this.adoptionForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      // phone: ['', [Validators.required, Validators.pattern('^\\+?49[1-9][0-9]{1,14}$')]], // german phone number format
      streetAddress: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', [Validators.required, Validators.pattern('^[0-9]{5}$')]],
      adoptionReason: ['', [Validators.required, Validators.minLength(0)]]
    });
  }
  // it's used to get the route parameter from the URL
  ngOnInit() {
    const catId = this.route.snapshot.paramMap.get('id');
    if (catId) {
      this.catService.getCatById(catId).subscribe(cat => {
        this.cat = cat;
        this.checkIfUserHasApplied();
      });
    }
  }
  checkIfUserHasApplied() {
    const userString = localStorage.getItem('currentUser');
    if (userString) {
      const userId = JSON.parse(userString).id;
      this.catService.checkIfUserHasApplied(this.cat!?.id, userId).subscribe(hasApplied => {
        this.hasApplied = hasApplied;
        if (hasApplied) {
          this.adoptionForm.disable();
        }
      });
    }
  }

  submitApplication() {
    if (this.isSubmitting || this.hasApplied) return;
    this.isSubmitting = true;

    if (this.adoptionForm.valid && this.cat) {
      const userString = localStorage.getItem('currentUser');
      const userId = userString ? JSON.parse(userString).id : null;

      if (!userId) {
        throw new Error('User not authenticated');
      }
      const formValue = this.adoptionForm.value;
      const application: AdoptionApplication = {
        id: Date.now().toString(),
        userId: userId,
        catId: this.cat.id,
        fullName: formValue.fullName,
        email: formValue.email,
        phoneNo: formValue.phone,
        status: 'pending',
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
        location: formValue.location,
        reason: formValue.reason
      };
      console.log('Submitting adoption application:', application);

      this.catService.submitAdoptionApplication(application).subscribe({
        next: (success) => {
          if (success) {
            this.showSuccess = true;
            this.adoptionForm.reset();
            this.hasApplied = true;
            this.adoptionForm.disable();
            setTimeout(() => {
              this.showSuccess = false;
            }, 3000);
          }
        },
        error: (error) => {
          console.error('Error submitting application:', error);
        },
        complete: () => {
          this.isSubmitting = false;
        }
      });
    }
  }
}