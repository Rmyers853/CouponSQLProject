# CouponSQLProject
You can find the finished build here: https://myersrya.itch.io/grocery-price-comparer  
Make sure to give permission to access files so it can create GroceryPriceComparer.sqlite  
  
You can find the C# scripts I wrote in CouponSQLProject/Assets/Scripts  
  
In Scripts/SceneManagerScripts, there are two scripts which work as follows:  
  
&emsp;TableScreenManager.cs  
&emsp;&emsp;Populates all store names (converted from hex) from the Stores table into the scroll view on the View Stores Page  
  
&emsp;TitleScreenManager.cs  
&emsp;&emsp;Functions for buttons on the Title Screen (first screen the user sees when they open the app)  
  
In Scripts/SQLScripts, there are four scripts which work as follows:  
  
&emsp;SQLManager.cs  
&emsp;&emsp;Helper functions I created to open the database, create a table (will add more tables in future), and execute a SQL command  
  
&emsp;CreateStoreManager.cs  
&emsp;&emsp;Functions for the Create Store popup that takes in store name (converts to hex to prevent sql injections) and distance from house and adds to the Stores table using the lowest possible available addressId (primary key for Stores table)  
  
&emsp;EditStoreManager.cs  
&emsp;&emsp;Same functions as CreateStoreManager, except it pre populates input fields to store selected, then user can edit those input fields and replace store name and distance in Stores table  
  
&emsp;StoreButtonScript.cs  
&emsp;&emsp;Functions for each Store that allow user to delete the Store or go to Edit Store Page
