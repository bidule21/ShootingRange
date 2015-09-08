namespace ShootingRange.ConfigurationProvider
{
    public static class ServiceDeskConfigurationExtractor
    {
        public static string GetProgramName(this ServiceDeskConfiguration serviceDeskConfiguration, int programNumber)
        {
            ParticipationDescription pd = serviceDeskConfiguration.ParticipationDescriptions.GetByKey(programNumber);
            return pd == null ? string.Format("unknown ({0})", programNumber) : pd.ProgramName;
        }
    }
}