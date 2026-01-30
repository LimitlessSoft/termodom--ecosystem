import { useState, useCallback } from 'react'
import { adminApi, handleApiError } from '@/apis/adminApi'

export const useAiValidation = () => {
    const [isValidating, setIsValidating] = useState(false)
    const [isGenerating, setIsGenerating] = useState(false)
    const [validationResult, setValidationResult] = useState(null)
    const [generationResult, setGenerationResult] = useState(null)

    const validateField = useCallback(
        async (endpoint, value, context) => {
            setIsValidating(true)
            try {
                const response = await adminApi.post(endpoint, { value, context })
                setValidationResult(response.data)
                return response.data
            } catch (error) {
                handleApiError(error)
                return null
            } finally {
                setIsValidating(false)
            }
        },
        []
    )

    const generateContent = useCallback(
        async (endpoint, options) => {
            setIsGenerating(true)
            try {
                const response = await adminApi.post(endpoint, options || {})
                setGenerationResult(response.data)
                return response.data
            } catch (error) {
                handleApiError(error)
                return null
            } finally {
                setIsGenerating(false)
            }
        },
        []
    )

    const clearValidationResult = useCallback(() => {
        setValidationResult(null)
    }, [])

    const clearGenerationResult = useCallback(() => {
        setGenerationResult(null)
    }, [])

    return {
        validateField,
        generateContent,
        isValidating,
        isGenerating,
        validationResult,
        generationResult,
        clearValidationResult,
        clearGenerationResult,
    }
}
