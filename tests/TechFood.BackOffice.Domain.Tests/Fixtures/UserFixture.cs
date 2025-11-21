using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.ValueObjects;

namespace TechFood.BackOffice.Domain.Tests.Fixtures;

public static class UserFixture
{
    public static User CreateValid(
        string fullName = "Admin User",
        string username = "admin",
        string role = "Administrator",
        string? email = "admin@techfood.com")
    {
        var name = new Name(fullName);
        var emailObj = email != null ? new Email(email) : null;

        return new User(name, username, role, emailObj);
    }

    public static User CreateValidWithoutEmail()
    {
        return CreateValid(
            fullName: "Manager User",
            username: "manager",
            role: "Manager",
            email: null);
    }

    public static User CreateWithCustomData(
        Name name,
        string username,
        string role,
        Email? email = null)
    {
        return new User(name, username, role, email);
    }

    public static User CreateOperatorUser()
    {
        return CreateValid(
            fullName: "Operator User",
            username: "operator",
            role: "Operator",
            email: "operator@techfood.com");
    }

    public static User CreateSuperAdminUser()
    {
        return CreateValid(
            fullName: "Super Admin",
            username: "superadmin",
            role: "SuperAdmin",
            email: "superadmin@techfood.com");
    }
}