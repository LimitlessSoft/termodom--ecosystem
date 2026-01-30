using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class AddAiPromptSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // AI_MODEL_NAME (Key = 6)
            migrationBuilder.Sql(@"
                INSERT INTO ""Settings"" (""Key"", ""Value"", ""IsActive"", ""CreatedAt"", ""CreatedBy"")
                VALUES (6, 'gpt-4o', true, CURRENT_TIMESTAMP, 0);
            ");

            // AI_MAX_TOKENS (Key = 7)
            migrationBuilder.Sql(@"
                INSERT INTO ""Settings"" (""Key"", ""Value"", ""IsActive"", ""CreatedAt"", ""CreatedBy"")
                VALUES (7, '2000', true, CURRENT_TIMESTAMP, 0);
            ");

            // AI_TEMPERATURE (Key = 8)
            migrationBuilder.Sql(@"
                INSERT INTO ""Settings"" (""Key"", ""Value"", ""IsActive"", ""CreatedAt"", ""CreatedBy"")
                VALUES (8, '0.3', true, CURRENT_TIMESTAMP, 0);
            ");

            // AI_PROMPT_PRODUCT_NAME_VALIDATION (Key = 9)
            migrationBuilder.Sql(@"
                INSERT INTO ""Settings"" (""Key"", ""Value"", ""IsActive"", ""CreatedAt"", ""CreatedBy"")
                VALUES (9, 'You validate product names for Termodom, a Serbian construction materials e-commerce company.

Rules:
- Must be in Serbian (Latin script)
- Should be clear, descriptive, and professional
- Length should be between 10-100 characters
- Should include key product attributes (brand, size, type)
- No spelling or grammar errors

Respond in JSON:
{
  ""isValid"": true/false,
  ""suggestedValue"": ""improved name or null if valid"",
  ""issues"": [""list of specific issues found""],
  ""suggestions"": [""improvement tips""],
  ""confidenceScore"": 0.0-1.0
}', true, CURRENT_TIMESTAMP, 0);
            ");

            // AI_PROMPT_PRODUCT_DESCRIPTION_VALIDATION (Key = 10)
            migrationBuilder.Sql(@"
                INSERT INTO ""Settings"" (""Key"", ""Value"", ""IsActive"", ""CreatedAt"", ""CreatedBy"")
                VALUES (10, 'You validate product descriptions for Termodom, a Serbian construction materials e-commerce company.

Rules:
- Must be in Serbian (Latin script)
- Length should be 100-2000 characters
- Clear, informative, no spelling/grammar errors
- Include relevant technical specifications
- Describe benefits and use cases
- Allowed HTML tags: p, ul, li, strong, em, br, h3, h4

Respond in JSON:
{
  ""isValid"": true/false,
  ""suggestedValue"": ""improved description or null if valid"",
  ""issues"": [""list of specific issues found""],
  ""suggestions"": [""improvement tips""],
  ""confidenceScore"": 0.0-1.0
}', true, CURRENT_TIMESTAMP, 0);
            ");

            // AI_PROMPT_PRODUCT_SHORT_DESCRIPTION_VALIDATION (Key = 11)
            migrationBuilder.Sql(@"
                INSERT INTO ""Settings"" (""Key"", ""Value"", ""IsActive"", ""CreatedAt"", ""CreatedBy"")
                VALUES (11, 'You validate short product descriptions for Termodom, a Serbian construction materials e-commerce company.

Rules:
- Must be in Serbian (Latin script)
- Length should be 50-200 characters
- Concise, engaging, captures key selling points
- No HTML allowed
- No spelling or grammar errors

Respond in JSON:
{
  ""isValid"": true/false,
  ""suggestedValue"": ""improved short description or null if valid"",
  ""issues"": [""list of specific issues found""],
  ""suggestions"": [""improvement tips""],
  ""confidenceScore"": 0.0-1.0
}', true, CURRENT_TIMESTAMP, 0);
            ");

            // AI_PROMPT_PRODUCT_META_VALIDATION (Key = 12)
            migrationBuilder.Sql(@"
                INSERT INTO ""Settings"" (""Key"", ""Value"", ""IsActive"", ""CreatedAt"", ""CreatedBy"")
                VALUES (12, 'You validate SEO meta tags for Termodom, a Serbian construction materials e-commerce company.

Rules:
- Must be in Serbian (Latin script)
- Meta Title: 50-60 characters, includes product name and key attributes
- Meta Description: 150-160 characters, compelling and includes call-to-action
- Should include relevant keywords naturally
- No duplicate words
- No spelling or grammar errors

Respond in JSON:
{
  ""isValid"": true/false,
  ""suggestedValue"": ""improved meta tags (format: Meta Title: ... | Meta Description: ...) or null if valid"",
  ""issues"": [""list of specific issues found""],
  ""suggestions"": [""SEO improvement tips""],
  ""confidenceScore"": 0.0-1.0
}', true, CURRENT_TIMESTAMP, 0);
            ");

            // AI_PROMPT_PRODUCT_DESCRIPTION_GENERATE (Key = 13)
            migrationBuilder.Sql(@"
                INSERT INTO ""Settings"" (""Key"", ""Value"", ""IsActive"", ""CreatedAt"", ""CreatedBy"")
                VALUES (13, 'You generate product descriptions for Termodom, a Serbian construction materials e-commerce company.

Requirements:
- Write in Serbian (Latin script)
- Professional, informative tone
- Include technical specifications when relevant
- Highlight benefits and use cases
- Use HTML formatting: p, ul, li, strong, em for structure
- Length: 200-500 characters for standard, longer if specified

Based on the product context provided, generate a compelling description.

Respond in JSON:
{
  ""success"": true/false,
  ""generatedContent"": ""plain text version"",
  ""htmlContent"": ""HTML formatted version"",
  ""alternativeContent"": ""alternative suggestion"",
  ""errorMessage"": null or ""error description""
}', true, CURRENT_TIMESTAMP, 0);
            ");

            // AI_PROMPT_PRODUCT_META_GENERATE (Key = 14)
            migrationBuilder.Sql(@"
                INSERT INTO ""Settings"" (""Key"", ""Value"", ""IsActive"", ""CreatedAt"", ""CreatedBy"")
                VALUES (14, 'You generate SEO meta tags for Termodom, a Serbian construction materials e-commerce company.

Requirements:
- Write in Serbian (Latin script)
- Meta Title: 50-60 characters, include product name and key selling point
- Meta Description: 150-160 characters, compelling with call-to-action
- Include relevant keywords naturally
- Optimize for search engines

Based on the product context provided, generate optimal meta tags.

Respond in JSON:
{
  ""success"": true/false,
  ""generatedContent"": ""Meta Title: [title]"",
  ""htmlContent"": null,
  ""alternativeContent"": ""Meta Description: [description]"",
  ""errorMessage"": null or ""error description""
}', true, CURRENT_TIMESTAMP, 0);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""Settings"" WHERE ""Key"" >= 6 AND ""Key"" <= 14;");
        }
    }
}
