TblAdmin
========

A first learning project to get my feet wet in asp.Net MVC with EF.

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
- an in memory database is set up for integration testing of the filtering/sorting/pagination
- I am mocking EF's DbSet and injecting it into the controllers for testing, rather than creating an additional repository layer and mocking up repository stubs. I am not worried about abstracting away the ORM at this time.
- the database is dropped/recreated and re-seeded automatically on every model change during development, to keep things simple so we don't have to mess around with migrations.
