import {AbstractControl, ValidationErrors, ValidatorFn} from '@angular/forms';

export class CustomValidators {
  public static Uppercase(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const hasUppercase = /[A-ZА-Я]/.test(control.value);
      return hasUppercase ? null : { uppercase: true };
    };
  }

  public static Lowercase(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const hasLowercase = /[a-zа-я]/.test(control.value);
      return hasLowercase ? null : { lowercase: true };
    };
  }

  public static Digit(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const hasDigit = /\d/.test(control.value);
      return hasDigit ? null : { digit: true };
    };
  }

  public static SpecialSymbol(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(control.value);
      return hasSpecialChar ? null : { specialChar: true };
    };
  }

  public static Compare(compareControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.parent) {
        return null;
      }

      const compareControl = control.parent.get(compareControlName);
      if (!compareControl) {
        return null;
      }

      return control.value == compareControl.value ? null : { compare: true };
    };
  }

  public static Duration(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      const regex = /^\d+:[0-5]\d$/;
      return regex.test(value) ? null : { duration: true };
    };
  }
}
