import { IQuizQuestion } from "@/widgets/Quiz/types/IQuizQuestion"
import { Quiz } from "@/widgets/Quiz/ui/Quiz"
import QuizQuestions from '@/app/quiz-questions.json'

const questions: IQuizQuestion[] = QuizQuestions

const Index = (): JSX.Element => {
    return (
        <Quiz questions={questions} />
    )
}

export default Index