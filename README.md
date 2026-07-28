# Product Catelog Management System (PCMS)

A full-stack application for managing products with features like search, category filtering, add/update forms, and delete functionality. 

The System comprises of 2 parts
- A Frontend client build on **Angular 20.3**
- Backend built with **ASP.NET MVC (WebAPI) + EF6 + MySQL**

---

## Project Structure
PCMS/

|--- PCMS-Angular/

|    		V---- PCMS‑FrontEnd/

|--- PCMS‑Backend/

---
## Prerequisites
- Nodejs (v18+ recommended)
- Angular CLI (v20.3) (`npm install -g @angular/cli`)
- .NET SDK (v6+)
- MySQL Server (5.7.18+)
- Visual Studio 
- VS Code

---

## Frontend Setup (Angular)
Open VS Code

1. Navigate to frontend folder:
	```bash
	cd PCMS/PCMS‑Angular/PCMS‑FrontEnd
	npm install
	``` 

2. Build 
	```bash
	npm run build 
	```
	OR
	```bash
	ng build
	```
3. Run locally
	```bash 
	ng serve
	```
	THEN 
	open http://localhost:4200 in your browser
	
4. Run unit tests
	```bash
	ng test
	```
	
---

## Backend Setup (ASP.NET MVC+ EFC and WebAPI)

1. Open the backend project in Visual Studio
2. Restore Nuget packages
	```bash
	dotnet restore
	```
3. Update connectionstring in appsettings.json with your **MySQL** connection string
	"ConnectionStrings": {
	  "DefaultConnection": "server=localhost;port=3306;database=ProductsDb;user=root;password=yourpassword;"
	}
4. Apply entity framework core migrations
	```bash
	dotnet ef database update
	```
5. Start the backend service
	```bash
	dotnet run
	```

---

### Additional Notes

- 	Angular Frontend client might need it's api path set in the encironment.ts file to match the localhost adress where the Backend
	decided to Start, this can be fount at ~/PCMS/PCMS‑Angular/PCMS‑FrontEnd/src/app/environment.ts

- 	AI Usage:
	-- Agent : Built in VSCode AI Agent
	Prompt : i need to generate atleast 1 unit test for each component and each service or page
	
	Response : 
			-- I’m reviewing the existing component and service specs now so I can add the missing unit tests in a consistent way and verify them.
			
			-- The current specs are very minimal, so I’m filling in missing cases around component behavior and service methods while making sure the test environment matches the standalone Angular setup.

			-- I’ve narrowed this to the existing standalone components and services, and I’m adjusting the specs to exercise their actual behavior rather than just checking creation.
			
			-- I’m writing the new tests around real component interactions now so each spec validates behavior rather than just instantiation.

			-- The tests are in place, and I’m running the suite now to catch any Angular-specific issues and adjust them if needed.
			
			-- Ran 
			```bash
				npm test -- --watch=false --browsers=ChromeHeadless
			```
			
			-- The first pass exposed a typing mismatch from the product model, and I’m aligning the test doubles with the actual interface so the suite compiles cleanly.
			
			-- Reviewed `product.model.ts` and updated `product-form.component.spec.ts`
			
			-- Ran
			```bash
				npm test -- --watch=false --browsers=ChromeHeadless
			```
			
			-- Corrected app component tests
			
			-- The remaining failure is in the app-level spec rather than the new component tests, and I’m updating that test harness so Angular has the router dependencies it expects.
			
			-- I’m patching the app component spec to provide the router setup Angular needs for the standalone app shell.
			
			-- Ran 
			```bash
				npm test -- --watch=false --browsers=ChromeHeadless
			```
			
			-- The test run is green now, and I’m wrapping up with the files updated and the verification results.
			
			-- I created at least one meaningful unit test for each component/service in the Angular app, including:
			```bash
				- Components:
					-- category-filter.component.spec.ts
					-- product-form.component.spec.ts
					-- product-list.component.spec.ts
					-- search-bar.component.spec.ts
				- Services:
					-- category.svc.spec.ts
					-- product.svc.spec.ts
				- App Shell:
					-- app.component.spec.ts
			```
			
			-- Verification
				--- I verified the suite by running 
				```bash
					npm test -- --watch=false --browsers=ChromeHeadless
				```
			
			-- Results
				---	13 tests executed
				--- 13 passed
				--- 0 failed	
					
	Prompt : Generate appealing stylesheets for all relevant components and pages
	
	Response : 
			-- I’ll inspect the Angular app structure and identify the existing components/pages so I can add matching styles.
			
			... Built in VS AI generated relevant stylesheets for created components
				
