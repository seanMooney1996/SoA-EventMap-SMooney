using SoACA1v2.DataModels;

namespace SoCA1v2Test.fakeObjects;

public static class FakeTicketMasterResponse
{
    public static readonly RootObject FakeResponse = new RootObject
    {
        Embedded = new Embedded
        {
            Events = new[]
            {
                new Event
                {
                    Name = "test event",
                    Url = "testurl",
                    Dates = new Dates
                    {
                        Start = new Start
                        {
                            LocalDate = "testdate",
                            LocalTime = "testtime"
                        }
                    },
                    Images = new[]
                    {
                        new Images { Url = "test" },
                        new Images { Url = "test" }
                    },
                    Embedded = new EventEmbedded
                    {
                        Venues = new[]
                        {
                            new Venue
                            {
                                Name = "test Venue",
                                City = new City { Name = "test" },
                                Location = new Location
                                {
                                    Latitude = "1.1",
                                    Longitude = "1.1"
                                },
                                Images = new[]
                                {
                                    new Images { Url = "test" }
                                }
                            }
                        }
                    }
                }
            }
        }
    };
}