import { render, screen, fireEvent } from '@testing-library/react';
import { ConfirmDialog } from './ConfirmDialog';

describe('ConfirmDialog', () => {
  const mockOnCancel = jest.fn();
  const mockOnConfirm = jest.fn();

  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('renders with default title when no title prop is provided', () => {
    render(
      <ConfirmDialog
        isOpen={true}
        onCancel={mockOnCancel}
        onConfirm={mockOnConfirm}
      />
    );

    expect(screen.getByText('Da li ste sigurni?')).toBeInTheDocument();
  });

  it('renders with custom title when title prop is provided', () => {
    const customTitle = 'Custom Confirmation Title';
    render(
      <ConfirmDialog
        isOpen={true}
        onCancel={mockOnCancel}
        onConfirm={mockOnConfirm}
        title={customTitle}
      />
    );

    expect(screen.getByText(customTitle)).toBeInTheDocument();
  });

  it('renders message when message prop is provided', () => {
    const message = 'Are you sure you want to proceed?';
    render(
      <ConfirmDialog
        isOpen={true}
        onCancel={mockOnCancel}
        onConfirm={mockOnConfirm}
        message={message}
      />
    );

    expect(screen.getByText(message)).toBeInTheDocument();
  });

  it('does not render message when message prop is not provided', () => {
    const { container } = render(
      <ConfirmDialog
        isOpen={true}
        onCancel={mockOnCancel}
        onConfirm={mockOnConfirm}
      />
    );

    // DialogContent should not be rendered when there's no message
    const dialogContent = container.querySelector('.MuiDialogContent-root');
    expect(dialogContent).not.toBeInTheDocument();
  });

  it('renders cancel and confirm buttons', () => {
    render(
      <ConfirmDialog
        isOpen={true}
        onCancel={mockOnCancel}
        onConfirm={mockOnConfirm}
      />
    );

    expect(screen.getByText('Ipak odustani...')).toBeInTheDocument();
    expect(screen.getByText('Jesam, nastavi!')).toBeInTheDocument();
  });

  it('calls onCancel when cancel button is clicked', () => {
    render(
      <ConfirmDialog
        isOpen={true}
        onCancel={mockOnCancel}
        onConfirm={mockOnConfirm}
      />
    );

    const cancelButton = screen.getByText('Ipak odustani...');
    fireEvent.click(cancelButton);

    expect(mockOnCancel).toHaveBeenCalledTimes(1);
    expect(mockOnConfirm).not.toHaveBeenCalled();
  });

  it('calls onConfirm when confirm button is clicked', () => {
    render(
      <ConfirmDialog
        isOpen={true}
        onCancel={mockOnCancel}
        onConfirm={mockOnConfirm}
      />
    );

    const confirmButton = screen.getByText('Jesam, nastavi!');
    fireEvent.click(confirmButton);

    expect(mockOnConfirm).toHaveBeenCalledTimes(1);
    expect(mockOnCancel).not.toHaveBeenCalled();
  });

  it('does not render when isOpen is false', () => {
    render(
      <ConfirmDialog
        isOpen={false}
        onCancel={mockOnCancel}
        onConfirm={mockOnConfirm}
      />
    );

    // MUI Dialog doesn't render its content when open=false
    expect(screen.queryByText('Da li ste sigurni?')).not.toBeInTheDocument();
  });
});
