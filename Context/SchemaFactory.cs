using Microsoft.EntityFrameworkCore;
using Hospital.Context;
using System.Runtime.InteropServices;
using System;

public static class SchemaFactory {
    public static string ConnectionString { get; set;} = "";
    public static HospitalSchema CreateContext() {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseNpgsql(ConnectionString);
        
        HospitalSchema nodeContext = new HospitalSchema(optionsBuilder.Options);
        return nodeContext;
    }



    public static void ApplyMigrations() {
        var nodeContext= CreateContext();
        if(nodeContext != null) {

            try{
                nodeContext.Database.Migrate();
                nodeContext.Dispose();

            }
            catch(System.Exception e) {
                Console.WriteLine("Migration Error"+ e.Message);

            }
        }
    }


}