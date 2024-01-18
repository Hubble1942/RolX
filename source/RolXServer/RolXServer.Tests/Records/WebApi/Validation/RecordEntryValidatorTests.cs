// -----------------------------------------------------------------------
// <copyright file="RecordEntryValidatorTests.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Projects.DataAccess;
using RolXServer.Records.WebApi.Resource;

namespace RolXServer.Records.WebApi.Validation;

/// <summary>
/// Unit tests for the <see cref="RecordEntryValidator"/>.
/// </summary>
public sealed class RecordEntryValidatorTests
{
    private RecordEntryValidator sut = null!;
    private Subproject subproject = null!;
    private RolXContext context = null!;
    private Record record = null!;

    private static Subproject SeedSubproject => new Subproject
    {
        Id = 1,
        Number = 1,
        Name = "One",
        Activities = new List<Activity>
            {
                new Activity
                {
                    Number = 1,
                    Name = "One",
                    StartDate = new DateOnly(2019, 12, 17),
                },
                new Activity
                {
                    Number = 2,
                    Name = "Two",
                    StartDate = new DateOnly(2019, 12, 17),
                    EndedDate = new DateOnly(2019, 12, 19),
                },
            },
    };

    [SetUp]
    public void SetUp()
    {
        this.subproject = SeedSubproject;
        this.context = InMemory.ContextFactory(this.subproject)();

        this.record = new Record
        {
            Date = "2019-12-18",
        };

        this.sut = new RecordEntryValidator(this.record, this.context);
    }

    [TearDown]
    public void TearDown()
    {
        this.context.Dispose();
    }

    [Test]
    [TestCase(-12312312423432)]
    [TestCase(-42)]
    [TestCase(-1)]
    public async Task Duration_FailsWhenNegative(long value)
    {
        var model = new RecordEntry
        {
            Duration = value,
        };

        (await this.sut.TestValidateAsync(model)).ShouldHaveValidationErrorFor(x => x.Duration);
    }

    [Test]
    public async Task Duration_SucceedsWhenZero()
    {
        var model = new RecordEntry
        {
            Duration = 0,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Duration);
    }

    [Test]
    [TestCase(12312312423432)]
    [TestCase(42)]
    [TestCase(1)]
    public async Task Duration_SucceedsWhenPositive(long value)
    {
        var model = new RecordEntry
        {
            Duration = value,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Duration);
    }

    [Test]
    public async Task ActivityId_FailsWhenZero()
    {
        var model = new RecordEntry
        {
            ActivityId = 0,
        };

        (await this.sut.TestValidateAsync(model)).ShouldHaveValidationErrorFor(x => x.ActivityId);
    }

    [Test]
    public async Task ActivityId_FailsWhenActivityUnkownAndDurationNonZero()
    {
        var model = new RecordEntry
        {
            ActivityId = 31415,
            Duration = 42,
        };

        (await this.sut.TestValidateAsync(model)).ShouldHaveValidationErrorFor(x => x.ActivityId);
    }

    [Test]
    public async Task ActivityId_SucceedsWhenActivityUnkownButDurationIsZero()
    {
        var model = new RecordEntry
        {
            ActivityId = 31415,
            Duration = 0,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.ActivityId);
    }

    [Test]
    public async Task ActivityId_SucceedsWhenActivityIsKnownAndOpen()
    {
        var model = new RecordEntry
        {
            ActivityId = this.subproject.Activities[0].Id,
            Duration = 42,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.ActivityId);
    }

    [Test]
    public async Task ActivityId_SucceedsWhenActivityOpensToday()
    {
        this.record.Date = "2019-12-17";

        var model = new RecordEntry
        {
            ActivityId = this.subproject.Activities[1].Id,
            Duration = 42,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.ActivityId);
    }

    [Test]
    public async Task ActivityId_FailsWhenActivityEndedToday()
    {
        this.record.Date = "2019-12-19";

        var model = new RecordEntry
        {
            ActivityId = this.subproject.Activities[1].Id,
            Duration = 42,
        };

        (await this.sut.TestValidateAsync(model)).ShouldHaveValidationErrorFor(x => x.ActivityId);
    }

    [Test]
    public async Task ActivityId_FailsWhenActivityClosedYesterday()
    {
        this.record.Date = "2019-12-20";

        var model = new RecordEntry
        {
            ActivityId = this.subproject.Activities[1].Id,
            Duration = 42,
        };

        (await this.sut.TestValidateAsync(model)).ShouldHaveValidationErrorFor(x => x.ActivityId);
    }

    [Test]
    public async Task ActivityId_FailsWhenActivityOpensTomorrow()
    {
        this.record.Date = "2019-12-16";

        var model = new RecordEntry
        {
            ActivityId = this.subproject.Activities[0].Id,
            Duration = 42,
        };

        (await this.sut.TestValidateAsync(model)).ShouldHaveValidationErrorFor(x => x.ActivityId);
    }

    [Test]
    public async Task ActivityId_SucceedsWhenActivityIsClosedButDurationIsZero()
    {
        this.record.Date = "2019-12-16";

        var model = new RecordEntry
        {
            ActivityId = this.subproject.Activities[0].Id,
            Duration = 0,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.ActivityId);
    }

    [Test]
    public async Task Begin_SucceedsWhenNull()
    {
        var model = new RecordEntry
        {
            Begin = null,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Begin);
    }

    [Test]
    public async Task Begin_SucceedsWhenZero()
    {
        var model = new RecordEntry
        {
            Begin = 0,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Begin);
    }

    [Test]
    public async Task Begin_SucceedsWhenWithin24h()
    {
        var model = new RecordEntry
        {
            Begin = 12 * 3600,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Begin);

        model.Begin = 24 * 3600;
        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Begin);
    }

    [Test]
    public async Task Begin_FailsWhenAbove24h()
    {
        var model = new RecordEntry
        {
            Begin = (24 * 3600) + 1,
        };

        (await this.sut.TestValidateAsync(model)).ShouldHaveValidationErrorFor(x => x.Begin);
    }

    [Test]
    [TestCase(-1)]
    [TestCase(-11)]
    [TestCase(-111)]
    public async Task Begin_FailsWhenNegative(int value)
    {
        var model = new RecordEntry
        {
            Begin = value,
        };

        (await this.sut.TestValidateAsync(model)).ShouldHaveValidationErrorFor(x => x.Begin);
    }

    [Test]
    public async Task Pause_SucceedsWhenNull()
    {
        var model = new RecordEntry
        {
            Pause = null,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Pause);
    }

    [Test]
    public async Task Pause_FailsWhenNotNullButBeginIsNull()
    {
        var model = new RecordEntry
        {
            Begin = null,
            Pause = 3600,
        };

        (await this.sut.TestValidateAsync(model)).ShouldHaveValidationErrorFor(x => x.Pause);
    }

    [Test]
    public async Task Pause_SucceedsWhenZero()
    {
        var model = new RecordEntry
        {
            Begin = 0,
            Pause = 0,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Pause);
    }

    [Test]
    public async Task Pause_SucceedsWhenWithin24h()
    {
        var model = new RecordEntry
        {
            Begin = 0,
            Pause = 12 * 3600,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Pause);

        model.Pause = 24 * 3600;
        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Pause);
    }

    [Test]
    public async Task Pause_FailsWhenNegative()
    {
        var model = new RecordEntry
        {
            Begin = 0,
            Pause = -111,
        };

        (await this.sut.TestValidateAsync(model)).ShouldHaveValidationErrorFor(x => x.Pause);
    }

    [Test]
    public async Task BeginPlusPausePlusDuration_SucceedsWhenWithin24h()
    {
        var model = new RecordEntry
        {
            Duration = 11 * 3600,
            Begin = 12 * 3600,
            Pause = 1800,
        };

        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Duration);

        model.Pause = 3600;
        (await this.sut.TestValidateAsync(model)).ShouldNotHaveValidationErrorFor(x => x.Duration);
    }

    [Test]
    public async Task BeginPlusPausePlusDuration_FailsWhenAbove24h()
    {
        var model = new RecordEntry
        {
            Duration = 11 * 3600,
            Begin = 12 * 3600,
            Pause = 3601,
        };

        (await this.sut.TestValidateAsync(model)).ShouldHaveValidationErrorFor(x => x.Duration);
    }
}
