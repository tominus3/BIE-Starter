using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace BIE.E2ETests;

[TestClass]
public class LearnerE2ETests : BrowserTestBase
{
    [TestMethod]
    public void TouristAt30DaysRemainsApprovedAfterReload()
    {
        // Arrange
        Fill("Sam Browser", "Tourismus", "30");

        // Act
        Driver.FindElement(By.Id("submit")).Click();
        Wait.Until(d => !string.IsNullOrEmpty(d.FindElement(By.Id("result")).Text));

        // Assert
        StringAssert.Contains(Decision("approved").Text, "GENEHMIGT");

        // Act
        var caseId = Driver.FindElement(By.Id("result")).GetDomAttribute("data-application-id");
        Driver.Navigate().Refresh();
        var row = Wait.Until(d => d.FindElements(By.CssSelector("#application-list tr")).FirstOrDefault(r => r.GetDomAttribute("data-id") == caseId));
        var cells = row.FindElements(By.TagName("td"));
        
        // Assert
        Assert.AreEqual("Sam Browser", cells.ElementAt(1).Text);
        Assert.AreEqual("Orion", cells.ElementAt(2).Text);
        Assert.AreEqual("30 Tage", cells.ElementAt(3).Text);
        Assert.AreEqual("GENEHMIGT", cells.ElementAt(4).Text);
    }
}
