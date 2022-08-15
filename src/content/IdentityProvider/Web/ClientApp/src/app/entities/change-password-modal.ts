export interface ChangePasswordModal {
	isChangingPassword: boolean;
	currentPassword: string | null;
	newPassword: string;
	newPasswordConfirmation: string;
}
