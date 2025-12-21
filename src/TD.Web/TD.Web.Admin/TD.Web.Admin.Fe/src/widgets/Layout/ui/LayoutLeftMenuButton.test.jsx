import { render, screen, fireEvent } from '@testing-library/react';
import LayoutLeftMenuButton from './LayoutLeftMenuButton';

// Mock the styled component
jest.mock('../styled/LayoutLeftMenuButtonStyled', () => ({
  LayoutLeftMenuButtonStyled: ({ children, onClick }) => (
    <button onClick={onClick} data-testid="menu-button">
      {children}
    </button>
  ),
}));

describe('LayoutLeftMenuButton', () => {
  it('renders children correctly', () => {
    render(<LayoutLeftMenuButton>Click Me</LayoutLeftMenuButton>);

    expect(screen.getByText('Click Me')).toBeInTheDocument();
  });

  it('renders multiple children correctly', () => {
    render(
      <LayoutLeftMenuButton>
        <span>Icon</span>
        <span>Label</span>
      </LayoutLeftMenuButton>
    );

    expect(screen.getByText('Icon')).toBeInTheDocument();
    expect(screen.getByText('Label')).toBeInTheDocument();
  });

  it('calls onClick handler when clicked', () => {
    const mockOnClick = jest.fn();

    render(<LayoutLeftMenuButton onClick={mockOnClick}>Click Me</LayoutLeftMenuButton>);

    const button = screen.getByTestId('menu-button');
    fireEvent.click(button);

    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('does not throw error when clicked without onClick handler', () => {
    render(<LayoutLeftMenuButton>Click Me</LayoutLeftMenuButton>);

    const button = screen.getByTestId('menu-button');

    expect(() => {
      fireEvent.click(button);
    }).not.toThrow();
  });

  it('calls onClick multiple times when clicked multiple times', () => {
    const mockOnClick = jest.fn();

    render(<LayoutLeftMenuButton onClick={mockOnClick}>Click Me</LayoutLeftMenuButton>);

    const button = screen.getByTestId('menu-button');
    fireEvent.click(button);
    fireEvent.click(button);
    fireEvent.click(button);

    expect(mockOnClick).toHaveBeenCalledTimes(3);
  });

  it('renders without crashing when no children provided', () => {
    const { container } = render(<LayoutLeftMenuButton />);

    expect(container).toBeInTheDocument();
  });
});
