import { BlogoviFilter } from './BlogoviFilter'
import { StripedDataGrid } from '@/widgets/StripedDataGrid'
import { GridActionsCellItem } from '@mui/x-data-grid'
import { Chip, CircularProgress } from '@mui/material'
import { useEffect, useState } from 'react'
import { Delete, Edit, Publish, Unpublished } from '@mui/icons-material'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { BlogStatus, BlogStatusLabels } from '../../interfaces/IBlog'
import qs from 'qs'
import Grid2 from '@mui/material/Unstable_Grid2'
import { toast } from 'react-toastify'

export const BlogoviList = () => {
    const [filters, setFilters] = useState({
        searchFilter: '',
        statusesFilter: [BlogStatus.Draft, BlogStatus.Published],
    })
    const [blogs, setBlogs] = useState(null)
    const [isFetching, setIsFetching] = useState(false)

    const fetchBlogs = () => {
        setIsFetching(true)

        const params = {
            ...(filters.searchFilter.trim() && {
                searchFilter: filters.searchFilter,
            }),
            ...(filters.statusesFilter.length > 0 && {
                status: filters.statusesFilter,
            }),
        }

        adminApi
            .get('/blogs', {
                params,
                paramsSerializer: (params) =>
                    qs.stringify(params, { arrayFormat: 'repeat' }),
            })
            .then((response) => setBlogs(response.data?.payload || []))
            .catch((err) => handleApiError(err))
            .finally(() => setIsFetching(false))
    }

    useEffect(() => {
        fetchBlogs()
    }, [filters])

    const handlePublish = (id) => {
        adminApi
            .put(`/blogs/${id}/publish`)
            .then(() => {
                toast.success('Blog uspešno objavljen!')
                fetchBlogs()
            })
            .catch(handleApiError)
    }

    const handleUnpublish = (id) => {
        adminApi
            .put(`/blogs/${id}/unpublish`)
            .then(() => {
                toast.success('Blog uspešno povučen!')
                fetchBlogs()
            })
            .catch(handleApiError)
    }

    const handleDelete = (id) => {
        if (!confirm('Da li ste sigurni da želite da obrišete ovaj blog?')) {
            return
        }
        adminApi
            .delete(`/blogs/${id}`)
            .then(() => {
                toast.success('Blog uspešno obrisan!')
                fetchBlogs()
            })
            .catch(handleApiError)
    }

    const getStatusColor = (status) => {
        switch (status) {
            case BlogStatus.Draft:
                return 'warning'
            case BlogStatus.Published:
                return 'success'
            default:
                return 'default'
        }
    }

    return (
        <Grid2 container direction="column" p={2} gap={2}>
            <Grid2>
                <BlogoviFilter
                    isFetching={isFetching}
                    currentBlogs={blogs}
                    onPretrazi={(search, statuses) =>
                        setFilters({
                            searchFilter: search,
                            statusesFilter: statuses,
                        })
                    }
                />
            </Grid2>
            <Grid2>
                {isFetching ? (
                    <CircularProgress />
                ) : (
                    <StripedDataGrid
                        autoHeight
                        rows={blogs || []}
                        noRowsMessage="Nema dostupnih blogova za izabrani filter"
                        columns={[
                            {
                                field: 'title',
                                headerName: 'Naslov',
                                minWidth: 250,
                                flex: 1,
                            },
                            {
                                field: 'slug',
                                headerName: 'Slug',
                                minWidth: 150,
                                flex: 0.5,
                            },
                            {
                                field: 'status',
                                headerName: 'Status',
                                minWidth: 120,
                                renderCell: (params) => (
                                    <Chip
                                        label={BlogStatusLabels[params.value]}
                                        color={getStatusColor(params.value)}
                                        size="small"
                                    />
                                ),
                            },
                            {
                                field: 'publishedAt',
                                headerName: 'Datum objave',
                                minWidth: 150,
                                renderCell: (params) =>
                                    params.value
                                        ? new Date(params.value).toLocaleDateString('sr-RS')
                                        : '-',
                            },
                            {
                                field: 'createdAt',
                                headerName: 'Kreirano',
                                minWidth: 150,
                                renderCell: (params) =>
                                    new Date(params.value).toLocaleDateString('sr-RS'),
                            },
                            {
                                field: 'actions',
                                headerName: 'Akcije',
                                type: 'actions',
                                width: 150,
                                getActions: (params) => {
                                    const actions = [
                                        <GridActionsCellItem
                                            key={`edit-blog-${params.id}`}
                                            icon={<Edit />}
                                            label="Izmeni"
                                            onClick={() => {
                                                window.open(
                                                    `/blogovi/${params.id}`,
                                                    '_blank'
                                                )
                                            }}
                                        />,
                                    ]

                                    if (params.row.status === BlogStatus.Draft) {
                                        actions.push(
                                            <GridActionsCellItem
                                                key={`publish-blog-${params.id}`}
                                                icon={<Publish />}
                                                label="Objavi"
                                                onClick={() => handlePublish(params.id)}
                                            />
                                        )
                                    } else {
                                        actions.push(
                                            <GridActionsCellItem
                                                key={`unpublish-blog-${params.id}`}
                                                icon={<Unpublished />}
                                                label="Povuci"
                                                onClick={() => handleUnpublish(params.id)}
                                            />
                                        )
                                    }

                                    actions.push(
                                        <GridActionsCellItem
                                            key={`delete-blog-${params.id}`}
                                            icon={<Delete />}
                                            label="Obriši"
                                            onClick={() => handleDelete(params.id)}
                                        />
                                    )

                                    return actions
                                },
                            },
                        ]}
                        initialState={{
                            pagination: {
                                paginationModel: { page: 0, pageSize: 10 },
                            },
                        }}
                        getRowClassName={(params) =>
                            params.indexRelativeToCurrentPage % 2 === 0 ? 'even' : 'odd'
                        }
                        pageSizeOptions={[5, 10, 25]}
                    />
                )}
            </Grid2>
        </Grid2>
    )
}
