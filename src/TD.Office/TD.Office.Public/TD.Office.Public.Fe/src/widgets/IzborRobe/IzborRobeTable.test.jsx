import { render, screen, fireEvent, waitFor } from '@testing-library/react'
import { IzborRobeTable } from './IzborRobeTable'
import { ThemeProvider, createTheme } from '@mui/material/styles'

const theme = createTheme()

const renderWithTheme = (component) => {
    return render(<ThemeProvider theme={theme}>{component}</ThemeProvider>)
}

describe('IzborRobeTable', () => {
    const mockOnSelectRoba = jest.fn()
    const mockRoba = [
        { robaId: 1, katBr: 'KAT001', naziv: 'Roba 1', jm: 'kom' },
        { robaId: 2, katBr: 'KAT002', naziv: 'Roba 2', jm: 'kg' },
        { robaId: 3, katBr: 'KAT003', naziv: 'Roba 3', jm: 'l' },
    ]

    beforeEach(() => {
        jest.clearAllMocks()
    })

    describe('Cursor Styling', () => {
        it('applies pointer cursor to DataGrid root element', () => {
            const { container } = renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    filter={{}}
                />
            )

            const dataGrid = container.querySelector('.MuiDataGrid-root')
            expect(dataGrid).toBeInTheDocument()

            const styles = window.getComputedStyle(dataGrid)
            expect(styles.cursor).toBe('pointer')
        })

        it('applies pointer cursor to DataGrid cells', () => {
            const { container } = renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    filter={{}}
                />
            )

            const cells = container.querySelectorAll('.MuiDataGrid-cell')
            expect(cells.length).toBeGreaterThan(0)

            cells.forEach((cell) => {
                const styles = window.getComputedStyle(cell)
                expect(styles.cursor).toBe('pointer')
            })
        })

        it('applies pointer cursor to DataGrid column headers', () => {
            const { container } = renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    filter={{}}
                />
            )

            const columnHeaders = container.querySelectorAll(
                '.MuiDataGrid-columnHeader'
            )
            expect(columnHeaders.length).toBeGreaterThan(0)

            columnHeaders.forEach((header) => {
                const styles = window.getComputedStyle(header)
                expect(styles.cursor).toBe('pointer')
            })
        })

        it('applies pointer cursor to DataGrid rows', () => {
            const { container } = renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    filter={{}}
                />
            )

            const rows = container.querySelectorAll('.MuiDataGrid-row')
            expect(rows.length).toBeGreaterThan(0)

            rows.forEach((row) => {
                const styles = window.getComputedStyle(row)
                expect(styles.cursor).toBe('pointer')
            })
        })
    })

    describe('Basic Rendering', () => {
        it('renders DataGrid with correct columns', () => {
            const { container } = renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    filter={{}}
                />
            )

            expect(screen.getByText('RobaID')).toBeInTheDocument()
            expect(screen.getByText('KatBr')).toBeInTheDocument()
            expect(screen.getByText('Naziv')).toBeInTheDocument()

            // Should have at least 3 column headers
            const columnHeaders = container.querySelectorAll('.MuiDataGrid-columnHeader')
            expect(columnHeaders.length).toBeGreaterThanOrEqual(3)
        })

        it('renders all rows from roba data', () => {
            renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    filter={{}}
                />
            )

            expect(screen.getByText('Roba 1')).toBeInTheDocument()
            expect(screen.getByText('Roba 2')).toBeInTheDocument()
            expect(screen.getByText('Roba 3')).toBeInTheDocument()
        })
    })

    describe('Double Click Interaction', () => {
        it('calls onSelectRoba with correct robaId on cell double click', () => {
            const { container } = renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    filter={{}}
                />
            )

            // Find first data cell and double click it
            const firstCell = container.querySelector(
                '.MuiDataGrid-row .MuiDataGrid-cell'
            )
            fireEvent.doubleClick(firstCell)

            expect(mockOnSelectRoba).toHaveBeenCalledTimes(1)
            expect(mockOnSelectRoba).toHaveBeenCalledWith(1)
        })

        it('opens quantity dialog when inputKolicine is true', () => {
            const { container } = renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    inputKolicine={true}
                    filter={{}}
                />
            )

            // Find first data cell and double click it
            const firstCell = container.querySelector(
                '.MuiDataGrid-row .MuiDataGrid-cell'
            )
            fireEvent.doubleClick(firstCell)

            // Dialog should appear
            expect(screen.getByText('Unos kolicine')).toBeInTheDocument()
            expect(screen.getByText('Potvrdi')).toBeInTheDocument()
            expect(screen.getByText('Odustani')).toBeInTheDocument()
        })
    })

    describe('Quantity Input Dialog', () => {
        it('allows entering quantity and confirming', () => {
            const { container } = renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    inputKolicine={true}
                    filter={{}}
                />
            )

            // Double click to open dialog
            const firstCell = container.querySelector(
                '.MuiDataGrid-row .MuiDataGrid-cell'
            )
            fireEvent.doubleClick(firstCell)

            // Find quantity input field
            const quantityInput = screen.getByLabelText('Kolicina')
            expect(quantityInput).toHaveValue('1')

            // Change quantity
            fireEvent.change(quantityInput, { target: { value: '5' } })
            expect(quantityInput).toHaveValue('5')

            // Click confirm button
            const confirmButton = screen.getByText('Potvrdi')
            fireEvent.click(confirmButton)

            // Should call onSelectRoba with robaId and quantity
            expect(mockOnSelectRoba).toHaveBeenCalledWith(1, 5)
        })

        it('closes dialog when cancel button is clicked', async () => {
            const { container } = renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    inputKolicine={true}
                    filter={{}}
                />
            )

            // Double click to open dialog
            const firstCell = container.querySelector(
                '.MuiDataGrid-row .MuiDataGrid-cell'
            )
            fireEvent.doubleClick(firstCell)

            // Wait for dialog to appear
            const dialogTitle = await screen.findByText('Unos kolicine')
            expect(dialogTitle).toBeInTheDocument()

            // Click cancel button
            const cancelButton = screen.getByText('Odustani')
            fireEvent.click(cancelButton)

            // Wait for dialog to be removed
            await waitFor(() => {
                expect(screen.queryByText('Unos kolicine')).not.toBeInTheDocument()
            })
        })

        it('replaces comma with dot in quantity input', () => {
            const { container } = renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    inputKolicine={true}
                    filter={{}}
                />
            )

            // Double click to open dialog
            const firstCell = container.querySelector(
                '.MuiDataGrid-row .MuiDataGrid-cell'
            )
            fireEvent.doubleClick(firstCell)

            const quantityInput = screen.getByLabelText('Kolicina')

            // Enter value with comma
            fireEvent.change(quantityInput, { target: { value: '3,5' } })

            // Should convert comma to dot
            expect(quantityInput).toHaveValue('3.5')
        })
    })

    describe('Filtering', () => {
        it('filters roba by naziv', () => {
            const { rerender } = renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    filter={{ search: 'Roba 1' }}
                />
            )

            expect(screen.getByText('Roba 1')).toBeInTheDocument()
            expect(screen.queryByText('Roba 2')).not.toBeInTheDocument()
            expect(screen.queryByText('Roba 3')).not.toBeInTheDocument()
        })

        it('filters roba by katBr', () => {
            renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    filter={{ search: 'KAT002' }}
                />
            )

            expect(screen.queryByText('Roba 1')).not.toBeInTheDocument()
            expect(screen.getByText('Roba 2')).toBeInTheDocument()
            expect(screen.queryByText('Roba 3')).not.toBeInTheDocument()
        })

        it('shows all roba when no filter is provided', () => {
            renderWithTheme(
                <IzborRobeTable
                    roba={mockRoba}
                    onSelectRoba={mockOnSelectRoba}
                    filter={{}}
                />
            )

            expect(screen.getByText('Roba 1')).toBeInTheDocument()
            expect(screen.getByText('Roba 2')).toBeInTheDocument()
            expect(screen.getByText('Roba 3')).toBeInTheDocument()
        })
    })
})