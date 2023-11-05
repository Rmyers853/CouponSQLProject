# CouponSQLProject

You can find the C# scripts I wrote in CouponSQLProject/Assets/Scripts  
  
In Scripts/SceneManagerScripts, there are two scripts which work as follows:  
TableScreenManager.cs Populates all store names (converted from hex) from the Stores table into the scroll view on the View Stores Page  
TitleScreenManager.cs Functions for buttons on the Title Screen (first screen the user sees when they open the app)  
  
In Scripts/SQLScripts, there are four scripts which work as follows:  
SQLManager.cs Helper functions I created to open the database, create a table (will add more tables in future), and execute a SQL command  
CreateStoreManager.cs Functions for the Create Store popup that takes in store name (converts to hex to prevent sql injections) and distance from house and adds to the Stores table using the lowest possible available addressId (primary key for Stores table)  
EditStoreManager.cs Same functions as CreateStoreManager, except it pre populates input fields to store selected, then user can edit those input fields and replace store name and distance in Stores table  
StoreButtonScript.cs Functions for each Store that allow user to delete the Store or go to Edit Store Page  
