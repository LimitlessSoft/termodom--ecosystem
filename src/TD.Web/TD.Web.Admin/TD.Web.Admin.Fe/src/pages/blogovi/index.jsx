import { BlogoviList } from '@/widgets/Blogovi/BlogoviList'
import { BlogoviActionMenu } from '@/widgets/Blogovi/BlogoviActionMenu'

const Blogovi = () => {
    return (
        <div className="p-2">
            <BlogoviActionMenu />
            <BlogoviList />
        </div>
    )
}

export default Blogovi
