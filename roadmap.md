# Roadmap for Take-Home Assignment: Building a REST API

## Overview
Develop a .NET 8 RESTful API to upload, validate, process, and store JSON metadata files related to clinical trials. Ensure code quality, maintainability, and scalability by adhering to Clean Architecture principles. The solution should include containerization, testing, and API documentation.

---

## Key Phases

### Phase 1: Project Setup
1. **Initialize the Project:**
   - Create a new `.NET 8` Web API project.
   - Set up the project structure to follow Clean Architecture principles.

2. **Dependencies:**
   - Add required NuGet packages:
     - `Swashbuckle.AspNetCore` (for Swagger documentation).
     - `EntityFrameworkCore` and `EntityFrameworkCore.SqlServer` (for database).
     - `Newtonsoft.Json.Schema` (for JSON schema validation).
     - `Docker.DotNet` (for containerization).

3. **Containerization Setup:**
   - Create a `Dockerfile` to containerize the application.
   - Define a `.dockerignore` file to exclude unnecessary files from the container.

---

### Phase 2: Core Functionality Development
1. **File Upload API:**
   - Create an endpoint to upload files (`POST /api/files/upload`).
   - Implement file size limitation and `.json` file type validation.

2. **JSON Validation:**
   - Load the provided JSON schema as an embedded resource.
   - Validate uploaded JSON files against the schema.

3. **Data Transformation:**
   - Calculate the trial duration in days.
   - Add default `endDate` if not provided and `status` is "Ongoing."

4. **Data Storage:**
   - Use Entity Framework Code First to design a database schema.
   - Map JSON data to database tables.

5. **Data Retrieval Endpoints:**
   - Create endpoints for:
     - Fetching a specific record by ID (`GET /api/trials/{id}`).
     - Filtering records by query parameters (`GET /api/trials?status={status}`).

---

### Phase 3: Quality Assurance
1. **Testing:**
   - Write unit tests for critical components (e.g., file validation, data transformation).
   - Write integration tests for API endpoints.

2. **Error Handling and Logging:**
   - Implement global exception handling.
   - Integrate structured logging (e.g., using `Serilog`).

3. **API Documentation:**
   - Configure Swagger/OpenAPI for interactive API documentation.
   - Ensure all endpoints are well-documented.

---

### Phase 4: Finalization
1. **Containerization:**
   - Test the Dockerized application locally.
   - Verify that the containerized application works seamlessly.

2. **README.md:**
   - Provide setup instructions for running the project locally and using Docker.
   - Include instructions for API usage.

3. **Submission:**
   - Push the project to a GitHub repository.
   - Verify all files and functionality before sharing the GitHub link.

---

## Detailed Timeline
- **Day 1:** Project setup and dependencies installation.
- **Day 2:** Implement file upload, JSON validation, and schema integration.
- **Day 3:** Add data transformation and database storage functionality.
- **Day 4:** Build retrieval endpoints, tests, and logging.
- **Day 5:** Containerize the app, finalize documentation, and verify the solution.

---

## Checklist
- [ ] Project structure follows Clean Architecture.
- [ ] All functional requirements are implemented.
- [ ] Unit and integration tests ensure reliability.
- [ ] Docker container runs the application successfully.
- [ ] API documentation is comprehensive and clear.
- [ ] README.md includes setup and usage instructions.

---

Good luck with your implementation! 🚀