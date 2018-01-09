package ClassLib032;

public class Class001 {
    public static String property() {
        return "ClassLib032" + " " + ClassLib002.Class001.property() + " " + ClassLib022.Class001.property() + " " + ClassLib011.Class001.property();
    }
}
