TblAdmin
========

A sample learning project to get my feet wet in asp.Net MVC with EF.

A TDD approach is being used to build up a very simple prototype of an admin.

Built with .Net 4.5, asp.Net MVC 5.1, EF 6

So far, 
- the index page has filtering, column sorting, pagination.
- MVC areas are being used to organized the code.
- PagedList is being used for pagination.
- Autofac is being used for dependancy injection (IoC).
- Nunit is being used for unit testing.
- Moq is being used for mocking.
- Glimpse is being used for debugging and diagnostics in development.
- Elmah is used for error handling and reporting in staging, useful for environment related errors.
- A view model is being used for all crud operations.
- Filtering/sorting/pagination state is maintained in the query string throughout the admin
- an in memory database is set up for integration testing
- I am mocking EF's DbSet and injecting it into the controllers for testing, rather than creating an additional repository layer and mocking up repository stubs. I am not interested in abstracting away the EF at this time.
- the database is dropped/recreated and re-seeded automatically on every model change during development, to keep things simple so we don't have to mess around with migrations.
- Unit tests are written for all crud operations, including redirects and error handling
